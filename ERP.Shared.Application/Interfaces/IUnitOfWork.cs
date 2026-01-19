namespace ERP.Shared.Application.Interfaces;

public interface IUnitOfWork
{
    Task<TResult> ExecuteWithTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct = default);

    Task<TResult> ExecuteWithoutTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct = default);
}



