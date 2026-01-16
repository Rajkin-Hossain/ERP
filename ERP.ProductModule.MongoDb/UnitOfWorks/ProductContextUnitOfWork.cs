using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.MongoDb.Data;
using ERP.SharedKernal.Entities;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.ProductModule.MongoDb.UnitOfWorks;

public sealed class ProductContextUnitOfWork(ProductDbContext dbContext) : IUnitOfWork
{
    public async Task<TResult> StartTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct = default)
    {
        // Mongo EF provider may or may not support EF transactions.
        // We attempt it; if not supported, we run without a transaction.
        try
        {
            /*await using var tx = await dbContext.Database.BeginTransactionAsync(ct);

            var result = await dbAction(ct);

            await AddTrackedDomainEvents(ct);
            await dbContext.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);

            ClearTrackedDomainEvents();
            return result;*/

            return await ExecuteWithoutTransactionAsync(dbAction, ct);
        }
        catch (NotSupportedException)
        {
            return await ExecuteWithoutTransactionAsync(dbAction, ct);
        }
        catch (InvalidOperationException)
        {
            // Some providers throw InvalidOperationException instead of NotSupportedException
            return await ExecuteWithoutTransactionAsync(dbAction, ct);
        }
    }

    private async Task<TResult> ExecuteWithoutTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct)
    {
        var result = await dbAction(ct);

        await AddTrackedDomainEvents(ct);
        await dbContext.SaveChangesAsync(ct);

        ClearTrackedDomainEvents();
        return result;
    }

    private async Task AddTrackedDomainEvents(CancellationToken ct)
    {
        var entities = GetTrackedBaseEntities();
        if (!entities.Any()) return;

        var outboxBatch = new List<ProductOutboxMessage>(
            capacity: entities.Sum(e => e.DomainEvents.Count));

        foreach (var entity in entities)
        {
            foreach (var ev in entity.DomainEvents)
            {
                outboxBatch.Add(ProductOutboxMessage.Create(entity.Id, ev));
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
