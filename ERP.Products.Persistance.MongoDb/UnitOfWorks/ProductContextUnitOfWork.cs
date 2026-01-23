using ERP.Products.Domain.ValueObjects;
using ERP.Products.Persistance.MongoDb.Data;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Domain.Entities;
using ERP.Shared.Domain.OutboxEntity;
namespace ERP.Products.Persistance.MongoDb.UnitOfWorks;

public sealed class ProductContextUnitOfWork(ProductDbContext dbContext) : IUnitOfWork
{
    public async Task<TResult> ExecuteWithTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct = default)
    {
        // Mongo EF provider for MongoDB supports transactions in a Replica Set environment.
        try
        {
            await using var tx = await dbContext.Database.BeginTransactionAsync(ct);

            var result = await dbAction(ct);
            //this var results now hold the AppResult<ProductId>.Ok object.
            //It will return when below cases are done successfully.

            await AddTrackedDomainEvents(ct);
            await dbContext.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);

            ClearTrackedDomainEvents();
            return result;
        }
        catch (Exception ex) when (ex is NotSupportedException or InvalidOperationException)
        {
            // Fallback for non-transactional environments (e.g., Standalone MongoDB instace)
            return await ExecuteWithoutTransactionAsync(dbAction, ct);
        }
    }

    public async Task<TResult> ExecuteWithoutTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct)
    {
        var result = await dbAction(ct);

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







