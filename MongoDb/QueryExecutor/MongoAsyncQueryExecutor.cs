using ERP.SharedKernal.Interfaces;
using MongoDB.Driver.Linq;

namespace MongoDb.QueryExecutor;

public sealed class MongoAsyncQueryExecutor : IAsyncQueryExecutor
{
    public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct = default)
    {
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken ct = default)
    {
        return await query.ToListAsync(ct);
    }
}
