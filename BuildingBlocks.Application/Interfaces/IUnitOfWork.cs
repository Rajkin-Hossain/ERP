namespace ERP.SharedKernal.Interfaces;

public interface IUnitOfWork
{
    Task<TResult> StartTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> dbAction,
        CancellationToken ct = default);
}
