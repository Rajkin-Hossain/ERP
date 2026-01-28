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
        var result = await operation(ct);

        await AddTrackedDomainEventsForId<AggRootId>(ct);
        await SaveChangesAsync(ct);
        ClearTrackedDomainEventsForId<AggRootId>();

        return result;
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default)
    {
        var result = await operation(ct);

        await AddTrackedDomainEventsForAllAggregates(ct);
        await SaveChangesAsync(ct);
        ClearTrackedDomainEventsForAllAggregates();

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
