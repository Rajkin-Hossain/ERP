using ERP.ProductModule.MongoDb.Data;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ERP.ProductModule.MongoDb.UnitOfWorks;

public sealed class ProductContextUnitOfWork(ProductDbContext dbContext) : IUnitOfWork
{
    public async Task<TResult> StartTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(dbAction);

        IDbContextTransaction? transaction = null;

        try
        {
            transaction = await dbContext.Database.BeginTransactionAsync(ct);
            var result = await dbAction(ct);
            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return result;
        }
        catch (NotSupportedException)
        {
            var result = await dbAction(ct);
            await dbContext.SaveChangesAsync(ct);
            return result;
        }
        catch
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(ct);
            }

            throw;
        }
        finally
        {
            if (transaction is not null)
            {
                await transaction.DisposeAsync();
            }
        }
    }
}
