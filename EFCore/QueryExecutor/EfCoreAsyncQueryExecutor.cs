using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCore.QueryExecutor;

public sealed class EfCoreAsyncQueryExecutor : IAsyncQueryExecutor
{
    public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken ct = default)
        => EntityFrameworkQueryableExtensions.ToListAsync(query, ct);

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct = default)
        => EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, ct);
}
