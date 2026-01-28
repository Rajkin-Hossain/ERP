namespace ERP.Shared.Application.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default);

    Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkExecutionOptions options,
        CancellationToken ct = default);

    Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default) where AggRootId : notnull;

    Task<TResult> ExecuteForAggregateAsync<AggRootId, TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        UnitOfWorkExecutionOptions options,
        CancellationToken ct = default) where AggRootId : notnull;
}



