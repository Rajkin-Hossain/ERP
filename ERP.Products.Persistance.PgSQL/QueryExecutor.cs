using ERP.Shared.Application.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.Products.Persistance.PgSQL;

public sealed class QueryExecutor : IQueryExecutor
{
    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        return query.FirstOrDefaultAsync(ct);
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        return query.ToListAsync(ct);
    }
}







