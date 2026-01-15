using ERP.SharedKernal.Entities;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PTE_Essay.Shared.Paging;
using System.Linq.Expressions;

namespace EFCore;

public class EFCoreRepositoryBase<T, TId>(DbContext dbContext) : IRepositoryBase<T, TId>
    where T : Entity<TId>
    where TId : notnull
{
    private static Expression<Func<T, bool>> IdEquals(TId id)
        => x => EqualityComparer<TId>.Default.Equals(x.Id, id);

    // -------------------------
    // Dynamic Query
    // -------------------------
    public IQueryable<T> GetConditional(Expression<Func<T, bool>>? predicate = null)
    {
        var q = dbContext.Set<T>().AsQueryable();
        return predicate is null ? q : q.Where(predicate);
    }

    public IQueryable<T> GetNoTrackingConditional(Expression<Func<T, bool>>? predicate = null)
    {
        var q = dbContext.Set<T>().AsNoTracking();
        return predicate is null ? q : q.Where(predicate);
    }

    public IQueryable<T> GetSingleConditional(Expression<Func<T, bool>> predicate)
        => GetConditional(predicate).Take(1);

    // -------------------------
    // Read Tracking
    // -------------------------
    public async Task<T?> FindAsync(TId id, CancellationToken ct = default)
        => await GetConditional(IdEquals(id)).FirstOrDefaultAsync(ct);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
        => await GetConditional(predicate).FirstOrDefaultAsync(ct);

    public async Task<List<T>> FetchModelsByIdsAsync(TId[] ids, CancellationToken ct = default)
    {
        if (ids is null || ids.Length == 0) return [];
        return await GetConditional(x => ids.Contains(x.Id)).ToListAsync(ct);
    }

    // -------------------------
    // Read No Tracking
    // -------------------------
    public async Task<T?> FindNoTrackingAsync(TId id, CancellationToken ct = default)
        => await GetNoTrackingConditional(IdEquals(id)).FirstOrDefaultAsync(ct);

    public async Task<T?> FirstOrDefaultNoTrackingAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
        => await GetNoTrackingConditional(predicate).FirstOrDefaultAsync(ct);

    public async Task<List<T>> FetchModelsByIdsNoTrackingAsync(TId[] ids, CancellationToken ct = default)
    {
        if (ids is null || ids.Length == 0) return [];
        return await GetNoTrackingConditional(x => ids.Contains(x.Id)).ToListAsync(ct);
    }

    public async Task<TResult?> FindAsync<TResult>(TId id, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        return await GetConditional(IdEquals(id)).Select(selector).FirstOrDefaultAsync(ct);
    }

    public async Task<TResult?> FindNoTrackingAsync<TResult>(TId id, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        return await GetNoTrackingConditional(IdEquals(id)).Select(selector).FirstOrDefaultAsync(ct);
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(
        Expression<Func<T, bool>>? predicate,
        Expression<Func<T, TResult>> selector,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        return await GetConditional(predicate).Select(selector).FirstOrDefaultAsync(ct);
    }

    public async Task<TResult?> FirstOrDefaultNoTrackingAsync<TResult>(
        Expression<Func<T, bool>>? predicate,
        Expression<Func<T, TResult>> selector,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        return await GetNoTrackingConditional(predicate).Select(selector).FirstOrDefaultAsync(ct);
    }

    public async Task<List<TResult>> FetchModelsByIdsAsync<TResult>(
        TId[] ids,
        Expression<Func<T, TResult>> selector,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (ids is null || ids.Length == 0) return [];
        return await GetConditional(x => ids.Contains(x.Id)).Select(selector).ToListAsync(ct);
    }

    public async Task<List<TResult>> FetchModelsByIdsNoTrackingAsync<TResult>(
        TId[] ids,
        Expression<Func<T, TResult>> selector,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (ids is null || ids.Length == 0) return [];
        return await GetNoTrackingConditional(x => ids.Contains(x.Id)).Select(selector).ToListAsync(ct);
    }

    // -------------------------
    // Insert
    // -------------------------
    public async Task InsertAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        await dbContext.Set<T>().AddAsync(model, cancellationToken: ct);
    }

    public async Task InsertRangeAsync(IEnumerable<T> models, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(models);
        await dbContext.Set<T>().AddRangeAsync(models.Where(m => m != null), cancellationToken: ct);
    }

    //Update
    public async Task UpdateAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        dbContext.Set<T>().Update(model);
        await Task.CompletedTask;
    }

    //Delete
    public async Task DeleteAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        dbContext.Set<T>().Remove(model);
        await Task.CompletedTask;
    }

    // -------------------------
    // Execute Update / Delete
    // -------------------------
    public async Task<int> ExecuteUpdateAsync(
        Expression<Func<T, bool>> predicate,
        Action<UpdateSettersBuilder<T>> setPropertyCalls,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(setPropertyCalls);

        return await GetConditional(predicate).ExecuteUpdateAsync(setPropertyCalls, ct);
    }

    public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return predicate is null ? throw new ArgumentNullException(nameof(predicate)) : await GetConditional(predicate).ExecuteDeleteAsync(ct);
    }

    //Pagination
    public async Task<PagedResult<T>> ToPagedResultAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        IQueryable<T> query = source;

        var totalCount = await query.CountAsync(ct);

        if (totalCount == 0)
            return PagedResult<T>.Create([], pageNumber, pageSize, 0);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return PagedResult<T>.Create(items, pageNumber, pageSize, totalCount);
    }

    public async Task<PagedResult<TResult>> ToPagedResultAsync<TResult>(
        IQueryable<T> source,
        Expression<Func<T, TResult>> selector,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        IQueryable<T> query = source;

        var totalCount = await query.CountAsync(ct);

        if (totalCount == 0)
            return PagedResult<TResult>.Create([], pageNumber, pageSize, 0);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(ct);

        return PagedResult<TResult>.Create(items, pageNumber, pageSize, totalCount);
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>>? predicate, CancellationToken token)
    {
        return await GetConditional(predicate)
            .AnyAsync(cancellationToken: token);
    }
}
