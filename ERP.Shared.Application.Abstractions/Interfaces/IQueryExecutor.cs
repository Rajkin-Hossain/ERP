namespace ERP.Shared.Application.Abstractions.Interfaces;

public interface IQueryExecutor
{
    Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken ct = default);
}




