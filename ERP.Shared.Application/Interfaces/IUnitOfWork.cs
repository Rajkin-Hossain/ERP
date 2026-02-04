namespace ERP.Shared.Application.Interfaces;

public interface IUnitOfWork
{
    Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default);

    Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkBehavior behavior,
        CancellationToken ct = default);

    Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default) where AggRootId : notnull;

    Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkBehavior behavior,
        CancellationToken ct = default) where AggRootId : notnull;
}

public enum UnitOfWorkBehavior
{
    None = 0, //For Example: ExecuteUpdateAsync/ExecuteDeleteAsync operations
    ChangeTracker = 1, //In General purpose where we need save changes only
    ChangeTrackerWithOutbox = 2, //When we need to save changes along with outbox messages
    Default = ChangeTrackerWithOutbox
}



