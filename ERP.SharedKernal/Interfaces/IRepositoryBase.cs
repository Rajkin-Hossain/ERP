using ERP.SharedKernal.Entities;
using PTE_Essay.Shared.Paging;
using System.Linq.Expressions;

namespace ERP.SharedKernal.Interfaces;

public interface IRepositoryBase<T, TId> where T : Entity<TId> where TId : notnull
{
    // Query
    IQueryable<T> GetConditional(Expression<Func<T, bool>>? predicate = null);
    IQueryable<T> GetNoTrackingConditional(Expression<Func<T, bool>>? predicate = null);
    IQueryable<T> GetSingleConditional(Expression<Func<T, bool>> predicate);

    // Read
    Task<T?> FindAsync(TId id, CancellationToken ct = default);
    Task<T?> FindNoTrackingAsync(TId id, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);
    Task<T?> FirstOrDefaultNoTrackingAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);

    Task<bool> ExistAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);

    Task<List<T>> FetchModelsByIdsAsync(TId[] ids, CancellationToken ct = default);
    Task<List<T>> FetchModelsByIdsNoTrackingAsync(TId[] ids, CancellationToken ct = default);

    Task<TResult?> FindAsync<TResult>(TId id, Expression<Func<T, TResult>> selector, CancellationToken ct = default);
    Task<TResult?> FindNoTrackingAsync<TResult>(TId id, Expression<Func<T, TResult>> selector, CancellationToken ct = default);
    Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TResult>> selector, CancellationToken ct = default);
    Task<TResult?> FirstOrDefaultNoTrackingAsync<TResult>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TResult>> selector, CancellationToken ct = default);

    Task<List<TResult>> FetchModelsByIdsAsync<TResult>(TId[] ids, Expression<Func<T, TResult>> selector, CancellationToken ct = default);
    Task<List<TResult>> FetchModelsByIdsNoTrackingAsync<TResult>(TId[] ids, Expression<Func<T, TResult>> selector, CancellationToken ct = default);

    // Insert (tracked)
    void Insert(T model);
    void InsertRange(IEnumerable<T> models);

    //Update
    void Update(T model);

    //Delete
    void Delete(T model);

    // Raw SQL
    Task<int?> ExecuteQueryAsync(string sql, CancellationToken ct = default);

    // Pagination
    Task<PagedResult<T>> ToPagedResultAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);

    Task<PagedResult<TResult>> ToPagedResultAsync<TResult>(
        IQueryable<T> source,
        Expression<Func<T, TResult>> selector,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);
}
