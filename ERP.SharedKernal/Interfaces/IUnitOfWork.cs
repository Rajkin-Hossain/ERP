namespace ERP.SharedKernal.Interfaces;

public interface IUnitOfWork
{
    Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
