using ERP.Shared.Application.Abstractions.Interfaces;
using ERP.Shared.Domain.Entities;
using ERP.Shared.Kernel.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP.Products.Persistance.PgSQL.BaseRepository.DbContexts.ProductDbContext.Write;

public abstract class PgRepository<T, TId>(DbContext dbContext) : IRepository<T, TId>
    where T : Entity<TId>
    where TId : notnull
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public IQueryable<T> GetConditional(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = _dbSet.AsQueryable();
        return predicate is null ? query : query.Where(predicate);
    }

    public IQueryable<T> GetSingleConditional(Expression<Func<T, bool>> predicate)
    {
        return GetConditional(predicate).Take(1);
    }

    public Task<T?> FindAsync(TId id, CancellationToken ct = default)
    {
        return _dbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id), ct);
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
        return predicate is null
            ? _dbSet.FirstOrDefaultAsync(ct)
            : _dbSet.FirstOrDefaultAsync(predicate, ct);
    }

    public Task<bool> ExistAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
        return predicate is null
            ? _dbSet.AnyAsync(ct)
            : _dbSet.AnyAsync(predicate, ct);
    }

    public Task<List<T>> FetchModelsByIdsAsync(TId[] ids, CancellationToken ct = default)
    {
        if (ids is null || ids.Length == 0)
        {
            return Task.FromResult<List<T>>([]);
        }

        return _dbSet.Where(entity => ids.Contains(entity.Id)).ToListAsync(ct);
    }

    public Task<TResult?> FindAsync<TResult>(TId id, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        return _dbSet.Where(entity => entity.Id.Equals(id)).Select(selector).FirstOrDefaultAsync(ct);
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        IQueryable<T> query = GetConditional(predicate);
        return query.Select(selector).FirstOrDefaultAsync(ct);
    }

    public Task<List<TResult>> FetchModelsByIdsAsync<TResult>(TId[] ids, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (ids is null || ids.Length == 0)
        {
            return Task.FromResult<List<TResult>>([]);
        }

        return _dbSet.Where(entity => ids.Contains(entity.Id)).Select(selector).ToListAsync(ct);
    }

    public async Task<AppPagedResult<T>> ToPagedResultAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        int totalCount = await source.CountAsync(ct);

        if (totalCount == 0)
        {
            return AppPagedResult<T>.Create([], pageNumber, pageSize, 0);
        }

        List<T> items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return AppPagedResult<T>.Create(items, pageNumber, pageSize, totalCount);
    }

    public async Task<AppPagedResult<TResult>> ToPagedResultAsync<TResult>(
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

        int totalCount = await source.CountAsync(ct);

        if (totalCount == 0)
        {
            return AppPagedResult<TResult>.Create([], pageNumber, pageSize, 0);
        }

        List<TResult> items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(ct);

        return AppPagedResult<TResult>.Create(items, pageNumber, pageSize, totalCount);
    }

    public Task<List<T>> ToListAsync(IQueryable<T> query, CancellationToken ct = default)
    {
        return query.ToListAsync(ct);
    }

    public Task<T?> FirstOrDefaultAsync(IQueryable<T> query, CancellationToken ct = default)
    {
        return query.FirstOrDefaultAsync(ct);
    }

    public Task InsertAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        return _dbSet.AddAsync(model, ct).AsTask();
    }

    public Task InsertRangeAsync(IEnumerable<T> models, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(models);
        return _dbSet.AddRangeAsync(models, ct);
    }

    public Task UpdateAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        _dbSet.Update(model);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        _dbSet.Remove(model);
        return Task.CompletedTask;
    }
}

