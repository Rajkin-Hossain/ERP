using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.ProductModule.MongoDb.QueryExecutor;

public sealed class EfCoreAsyncQueryExecutor : IAsyncQueryExecutor
{
    public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken ct = default)
    {
        return query.ToListAsync(ct);
    }

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct = default)
    {
        return query.FirstOrDefaultAsync(ct);
    }
}
