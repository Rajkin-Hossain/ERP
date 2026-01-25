using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Abstractions.Interfaces;
using ERP.Shared.Domain.Entities;
using ERP.Shared.Domain.OutboxEntity;

namespace ERP.Products.Persistance.PgSQL.UnitOfWorks.DbContexts.ProductDbContext.Write;

public sealed class ProductContextUnitOfWork(ProductDbContext dbContext) : IUnitOfWork
{
    public async Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default)
    {
        var result = await operation(ct);

        await AddTrackedDomainEvents(ct);

        await SaveChangesAsync(ct);

        ClearTrackedDomainEvents();

        return result;
    }

    private async Task SaveChangesAsync(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }

    private async Task AddTrackedDomainEvents(CancellationToken ct)
    {
        var entities = GetTrackedBaseEntities();

        if (!entities.Any()) return;

        var outboxBatch = new List<OutboxMessage>(
            capacity: entities.Sum(e => e.DomainEvents.Count));

        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                outboxBatch.Add(OutboxMessage.Create(entity.Id.Value, domainEvent));
            }
        }

        if (outboxBatch.Count > 0)
        {
            await dbContext.ProductOutboxMessages.AddRangeAsync(outboxBatch, ct);
        }
    }

    private void ClearTrackedDomainEvents()
    {
        var entities = GetTrackedBaseEntities();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }
    }

    private IEnumerable<AggregateRoot<ProductId>> GetTrackedBaseEntities()
    {
        return dbContext.ChangeTracker
            .Entries<AggregateRoot<ProductId>>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0);
    }
}







