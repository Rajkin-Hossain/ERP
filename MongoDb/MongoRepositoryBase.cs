namespace MongoDb;

using ERP.SharedKernal.Entities;
using ERP.SharedKernal.Interfaces;
using MongoDb.DbContext;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PTE_Essay.Shared.Paging;
using System.Linq.Expressions;

public abstract class MongoRepositoryBase<T, TId>(MongoDbContext dbContext) : IRepositoryBase<T, TId>
    where T : Entity<TId>
    where TId : notnull
{
    private readonly IMongoCollection<T> _collection = dbContext.Set<T>();

    // -------------------------
    // Query
    // -------------------------
    public IQueryable<T> GetConditional(Expression<Func<T, bool>>? predicate = null)
    {
        var query = _collection.AsQueryable();
        return predicate is null ? query : query.Where(predicate);
    }

    public IQueryable<T> GetSingleConditional(Expression<Func<T, bool>> predicate)
    {
        return GetConditional(predicate).Take(1);
    }

    // -------------------------
    // Read
    // -------------------------
    public async Task<T?> FindAsync(TId id, CancellationToken ct = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
        var query = predicate is null 
            ? _collection.AsQueryable() 
            : _collection.AsQueryable().Where(predicate);
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
         var query = predicate is null 
            ? _collection.AsQueryable() 
            : _collection.AsQueryable().Where(predicate);
        return await query.AnyAsync(ct);
    }

    public async Task<List<T>> FetchModelsByIdsAsync(TId[] ids, CancellationToken ct = default)
    {
        if (ids is null || ids.Length == 0) return [];
        var filter = Builders<T>.Filter.In(x => x.Id, ids);
        return await _collection.Find(filter).ToListAsync(ct);
    }

    public async Task<TResult?> FindAsync<TResult>(TId id, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        var query = _collection.AsQueryable().Where(x => x.Id.Equals(id));
        return await query.Select(selector).FirstOrDefaultAsync(ct);
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        var query = GetConditional(predicate);
        return await query.Select(selector).FirstOrDefaultAsync(ct);
    }

    public async Task<List<TResult>> FetchModelsByIdsAsync<TResult>(TId[] ids, Expression<Func<T, TResult>> selector, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (ids is null || ids.Length == 0) return [];
        return await _collection.AsQueryable()
            .Where(x => ids.Contains(x.Id))
            .Select(selector)
            .ToListAsync(ct);
    }

    // -------------------------
    // Insert
    // -------------------------
    public async Task InsertAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        await _collection.InsertOneAsync(model, cancellationToken: ct);
    }

    public async Task InsertRangeAsync(IEnumerable<T> models, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(models);
        var list = models.ToList();

        if (list.Count > 0)
        {
            await _collection.InsertManyAsync(list, cancellationToken: ct);
        }
    }

    // -------------------------
    // Update
    // -------------------------
    public async Task UpdateAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var filter = Builders<T>.Filter.Eq(x => x.Id, model.Id);
        await _collection.ReplaceOneAsync(filter, model, cancellationToken: ct);
    }

    // -------------------------
    // Delete
    // -------------------------
    public async Task DeleteAsync(T model, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        var filter = Builders<T>.Filter.Eq(x => x.Id, model.Id);
        await _collection.DeleteOneAsync(filter, cancellationToken: ct);
    }

    // -------------------------
    // Pagination
    // -------------------------
    public async Task<PagedResult<T>> ToPagedResultAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        var totalCount = await source.CountAsync(ct);

        if (totalCount == 0)
            return PagedResult<T>.Create([], pageNumber, pageSize, 0);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return PagedResult<T>.Create(items, pageNumber, pageSize, (int)totalCount);
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

        var totalCount = await source.CountAsync(ct);

        if (totalCount == 0)
            return PagedResult<TResult>.Create([], pageNumber, pageSize, 0);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(ct);

        return PagedResult<TResult>.Create(items, pageNumber, pageSize, (int)totalCount);
    }
}
