namespace ERP.Shared.Application.Abstractions.Interfaces;

[Flags]
public enum UnitOfWorkBehavior
{
    None = 0,
    ChangeTracker = 1,
    Outbox = 2,
    Default = ChangeTracker | Outbox
}

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



