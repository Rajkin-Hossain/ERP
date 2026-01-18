namespace ERP.Shared.Application.Interfaces;

public interface IUnitOfWork
{
    Task<TResult> StartTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct = default);
}



