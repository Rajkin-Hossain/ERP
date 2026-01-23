using ERP.Shared.Application.Paging;
using ERP.Shared.Domain.Entities;
using System.Linq.Expressions;

namespace ERP.Shared.Application.Interfaces;

public interface IRepository<T, TId> where T : Entity<TId> where TId : notnull
{
    // Query
    IQueryable<T> GetConditional(Expression<Func<T, bool>>? predicate = null);
    IQueryable<T> GetSingleConditional(Expression<Func<T, bool>> predicate);

    // Read
    Task<T?> FindAsync(TId id, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);

    Task<bool> ExistAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);

    Task<List<T>> FetchModelsByIdsAsync(TId[] ids, CancellationToken ct = default);

    Task<TResult?> FindAsync<TResult>(TId id, Expression<Func<T, TResult>> selector, CancellationToken ct = default);
    Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TResult>> selector, CancellationToken ct = default);

    Task<List<TResult>> FetchModelsByIdsAsync<TResult>(TId[] ids, Expression<Func<T, TResult>> selector, CancellationToken ct = default);

    // Pagination
    Task<AppPagedResult<T>> ToPagedResultAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);

    Task<AppPagedResult<TResult>> ToPagedResultAsync<TResult>(
        IQueryable<T> source,
        Expression<Func<T, TResult>> selector,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);

    Task InsertAsync(T model, CancellationToken ct = default);
    Task InsertRangeAsync(IEnumerable<T> models, CancellationToken ct = default);

    //Update
    Task UpdateAsync(T model, CancellationToken ct = default);

    //Delete
    Task DeleteAsync(T model, CancellationToken ct = default);
}




