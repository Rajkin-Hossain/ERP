using ERP.Shared.Application.Interfaces;

using Microsoft.EntityFrameworkCore;

using ERP.Products.Domain.Entities;
using ERP.Products.Application.Interfaces;
namespace ERP.Products.Persistance.MongoDb.QueryExecutor;

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







