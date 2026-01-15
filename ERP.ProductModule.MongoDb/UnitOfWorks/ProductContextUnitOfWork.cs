using ERP.SharedKernal.Exceptions;
using ERP.SharedKernal.Interfaces;
using MongoDB.Driver;

namespace ERP.ProductModule.MongoDb.UnitOfWorks;

public class ProductContextUnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IMongoClient _client;
    public IClientSessionHandle? Session { get; private set; }

    public ProductContextUnitOfWork(IMongoClient client)
    {
        _client = client;
    }

    public async Task<TResult> StartTransactionAsync<TResult>(Func<CancellationToken, 
        Task<TResult>> dbAction, 
        CancellationToken ct = default)
    {
        try
        {
            Session = await _client.StartSessionAsync(cancellationToken: ct);

            Session.StartTransaction();

            var result = await dbAction(ct);

            await CommitTransactionAsync(ct);

            return result;
        }
        catch (Exception ex)
        {
            if (Session != null && Session.IsInTransaction)
            {
                await Session.AbortTransactionAsync(ct);
            }

            throw new MongoDbException(ex.Message);
        }
    }

    private async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (Session != null && Session.IsInTransaction)
        {
            await Session.CommitTransactionAsync(ct);
        }
    }

    public void Dispose()
    {
        Session?.Dispose();
    }
}
