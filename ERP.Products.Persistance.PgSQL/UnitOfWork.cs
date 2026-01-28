using ERP.Products.Persistance.PgSQL.Data.Write;
using ERP.Shared.Application.Abstractions.Interfaces;
using ERP.Shared.Domain.Entities;
using ERP.Shared.Infrastructures.Outbox;
using System.Text.Json;

namespace ERP.Products.Persistance.PgSQL;

public sealed class UnitOfWork(ProductDbContext dbContext) : IUnitOfWork
{
    public async Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default) where AggRootId : notnull
    {
        return await ExecuteAsync(
            operation,
            UnitOfWorkExecutionOptions.Default,
            AddTrackedDomainEventsForId<AggRootId>,
            ClearTrackedDomainEventsForId<AggRootId>,
            ct);
    }

    public async Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkExecutionOptions options,
        CancellationToken ct = default) where AggRootId : notnull
    {
        return await ExecuteAsync(
            operation,
            options,
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
            UnitOfWorkExecutionOptions.Default,
            AddTrackedDomainEventsForAllAggregates,
            ClearTrackedDomainEventsForAllAggregates,
            ct);
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkExecutionOptions options,
        CancellationToken ct = default)
    {
        return await ExecuteAsync(
            operation,
            options,
            AddTrackedDomainEventsForAllAggregates,
            ClearTrackedDomainEventsForAllAggregates,
            ct);
    }

    private async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkExecutionOptions options,
        Func<CancellationToken, Task> addOutbox,
        Action clearOutbox,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(addOutbox);
        ArgumentNullException.ThrowIfNull(clearOutbox);

        var result = await operation(ct);

        if (options.UseChangeTracker)
        {
            if (options.UseOutbox)
                await addOutbox(ct);

            await SaveChangesAsync(ct);

            if (options.UseOutbox)
                clearOutbox();
        }
        else
        {
            await SaveChangesAsync(ct);
        }

        return result;
    }

    private Task SaveChangesAsync(CancellationToken ct)
        => dbContext.SaveChangesAsync(ct);

    private async Task AddTrackedDomainEventsForAllAggregates(CancellationToken ct)
    {
        var entities = GetTrackedAggregateRoots().ToList();
        if (entities.Count == 0) return;

        var outboxBatch = new List<OutboxMessage>(
            capacity: entities.Sum(e => e.DomainEvents.Count));

        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                var type = domainEvent.GetType();
                var eventType = type.FullName ?? type.Name ?? string.Empty;
                var payload = JsonSerializer.Serialize(domainEvent, type);

                outboxBatch.Add(
                    OutboxMessage.Create(entity.GetAggregateId(), eventType, payload));
            }
        }

        if (outboxBatch.Count > 0)
            await dbContext.ProductOutboxMessages.AddRangeAsync(outboxBatch, ct);
    }

    private void ClearTrackedDomainEventsForAllAggregates()
    {
        foreach (var entity in GetTrackedAggregateRoots())
            entity.ClearDomainEvents();
    }

    private IEnumerable<AggregateRootBase> GetTrackedAggregateRoots()
    {
        return dbContext.ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .OfType<AggregateRootBase>()
            .Where(e => e.DomainEvents.Count > 0);
    }

    private async Task AddTrackedDomainEventsForId<AggRootId>(CancellationToken ct) where AggRootId : notnull
    {
        var entities = GetTrackedBaseEntities<AggRootId>().ToList();
        if (entities.Count == 0) return;

        var outboxBatch = new List<OutboxMessage>(
            capacity: entities.Sum(e => e.DomainEvents.Count));

        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                var type = domainEvent.GetType();
                var eventType = type.FullName ?? type.Name ?? string.Empty;
                var payload = JsonSerializer.Serialize(domainEvent, type);

                outboxBatch.Add(
                    OutboxMessage.Create(entity.Id!.ToString()!, eventType, payload));
            }
        }

        if (outboxBatch.Count > 0)
            await dbContext.ProductOutboxMessages.AddRangeAsync(outboxBatch, ct);
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
}
