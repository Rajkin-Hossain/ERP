using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.SharedKernal.Interfaces;

namespace ERP.ProductModule.MongoDb.Outbox;

public class ProductOutboxJob
{
    private readonly IServiceBus _bus;
    private readonly IProductOutboxRepository _repo;

    public ProductOutboxJob(IServiceBus bus, 
        IProductOutboxRepository repo)
    {
        _bus = bus;
        _repo = repo;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {

    }
}
