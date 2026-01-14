using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.Infrastructure.Data.ProductContext;
using ERP.SharedKernal.Entities;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ERP.ProductModule.Infrastructure.UnitOfWorks.ProductContext;

public class ProductContextUnitOfWork(ProductDbContext dbContext) : IUnitOfWork
{
    public async Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken ct = default)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);

            var result = await action(ct);

            await AddTrackedDomainEvents(ct);

            await SaveChangesAsync(ct);

            await tx.CommitAsync(ct); // only if all succeeded

            ClearTrackedDomainEvents();

            return result;
        });
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);

    private async Task AddTrackedDomainEvents(CancellationToken ct = default)
    {
        var entities = GetTrackedBaseEntities();

        if (!entities.Any()) return;

        var outboxBatch = new List<ProductOutboxMessage>(capacity: entities.Sum(a => a.DomainEvents.Count));

        foreach (var entity in entities)
        {
            foreach (var ev in entity.DomainEvents)
            {
                outboxBatch.Add(ProductOutboxMessage.Create(entity.Id, ev));
            }
        }

        if (outboxBatch.Count > 0)
        {
            await dbContext.OutboxMessages.AddRangeAsync(outboxBatch, ct);
        }
    }

    private void ClearTrackedDomainEvents()
    {
        var entities = GetTrackedBaseEntities();

        if (!entities.Any()) return;

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }
    }

    private IEnumerable<AggregateRoot<ProductId>> GetTrackedBaseEntities()
    {
        var entities = dbContext.ChangeTracker.Entries<AggregateRoot<ProductId>>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0);

        return entities;
    }
}
