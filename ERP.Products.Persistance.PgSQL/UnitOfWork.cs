using ERP.Products.Persistance.PgSQL.Data.Write;
using ERP.Products.Persistance.PgSQL.Mappers;
using ERP.Products.Persistance.PgSQL.Outbox.Models;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Domain.Entities;
using ERP.Shared.Domain.Interfaces;
using System.Text.Json;

namespace ERP.Products.Persistance.PgSQL;

public sealed class UnitOfWork(
    ProductDbContext dbContext) : IUnitOfWork
{
    public async Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default) where AggRootId : notnull
    {
        return await ExecuteAsync(
            operation,
            UnitOfWorkBehavior.Default,
            AddTrackedDomainEventsForId<AggRootId>,
            ClearTrackedDomainEventsForId<AggRootId>,
            ct);
    }

    public async Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkBehavior behavior,
        CancellationToken ct = default) where AggRootId : notnull
    {
        return await ExecuteAsync(
            operation,
            behavior,
            AddTrackedDomainEventsForId<AggRootId>,
            ClearTrackedDomainEventsForId<AggRootId>,
            ct);
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default)
    {
        return await ExecuteAsync(
            operation,
            UnitOfWorkBehavior.Default,
            AddTrackedDomainEventsForAllAggregates,
            ClearTrackedDomainEventsForAllAggregates,
            ct);
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkBehavior behavior,
        CancellationToken ct = default)
    {
        return await ExecuteAsync(
            operation,
            behavior,
            AddTrackedDomainEventsForAllAggregates,
            ClearTrackedDomainEventsForAllAggregates,
            ct);
    }

    private async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkBehavior behavior,
        Func<CancellationToken, Task> addOutbox,
        Action clearOutbox,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(addOutbox);
        ArgumentNullException.ThrowIfNull(clearOutbox);

        var (useChangeTracker, useOutbox) = behavior switch
        {
            UnitOfWorkBehavior.None => (false, false),
            UnitOfWorkBehavior.ChangeTracker => (true, false),
            UnitOfWorkBehavior.ChangeTrackerWithOutbox => (true, true),
            _ => throw new ArgumentOutOfRangeException(nameof(behavior), behavior, "Unsupported unit of work behavior.")
        };

        var result = await operation(ct);

        if (useChangeTracker)
        {
            if (useOutbox)
                await addOutbox(ct);

            await SaveChangesAsync(ct);

            if (useOutbox)
                clearOutbox();
        }

        return result;
    }

    private Task SaveChangesAsync(CancellationToken ct)
        => dbContext.SaveChangesAsync(ct);

    private async Task AddTrackedDomainEventsForAllAggregates(CancellationToken ct)
    {
        var entities = GetTrackedAggregateRoots().ToList();
        if (entities.Count == 0) return;

        var outboxBatch = new List<OutboxMessage>();

        foreach (var entity in entities)
        {
            AddOutboxMessages(outboxBatch, entity.GetAggregateId(), entity.DomainEvents);
        }

        if (outboxBatch.Count > 0)
            await dbContext.OutboxMessages.AddRangeAsync(outboxBatch, ct);
    }

    private void ClearTrackedDomainEventsForAllAggregates()
    {
        foreach (var entity in GetTrackedAggregateRoots())
            entity.ClearDomainEvents();
    }

    private IEnumerable<IAggregateRoot> GetTrackedAggregateRoots()
    {
        return dbContext.ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .OfType<IAggregateRoot>()
            .Where(e => e.DomainEvents.Count > 0);
    }

    private async Task AddTrackedDomainEventsForId<AggRootId>(CancellationToken ct) where AggRootId : notnull
    {
        var entities = GetTrackedBaseEntities<AggRootId>().ToList();
        if (entities.Count == 0) return;

        var outboxBatch = new List<OutboxMessage>();

        foreach (var entity in entities)
        {
            AddOutboxMessages(outboxBatch, entity.Id!.ToString()!, entity.DomainEvents);
        }

        if (outboxBatch.Count > 0)
            await dbContext.OutboxMessages.AddRangeAsync(outboxBatch, ct);
    }

    private void ClearTrackedDomainEventsForId<AggRootId>() where AggRootId : notnull
    {
        foreach (var entity in GetTrackedBaseEntities<AggRootId>())
            entity.ClearDomainEvents();
    }

    private IEnumerable<AggregateRoot<AggRootId>> GetTrackedBaseEntities<AggRootId>() where AggRootId : notnull
    {
        return dbContext.ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .OfType<AggregateRoot<AggRootId>>()
            .Where(e => e.DomainEvents.Count > 0);
    }

    private void AddOutboxMessages(
        ICollection<OutboxMessage> outboxBatch,
        string aggregateId,
        IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        ArgumentNullException.ThrowIfNull(outboxBatch);
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        ArgumentNullException.ThrowIfNull(domainEvents);

        var integrationEvents = IntegrationEventMapper.Map(domainEvents);
        if (integrationEvents.Count == 0) return;

        foreach (var integrationEvent in integrationEvents)
        {
            var type = integrationEvent.GetType();
            var eventType = type.FullName ?? type.Name ?? string.Empty;
            var payload = JsonSerializer.Serialize(integrationEvent, type);

            outboxBatch.Add(OutboxMessage.Create(aggregateId, eventType, payload));
        }
    }
}
