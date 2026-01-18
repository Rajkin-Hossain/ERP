using BuildingBlocks.Application.Interfaces;
using ERP.Products.Application.Interfaces;

namespace ERP.Products.Messaging.RabbitMQ;

public class ProductOutboxDispatcher : IProductOutboxDispatcher
{
    private readonly IServiceBus _bus;

    public ProductOutboxDispatcher(IServiceBus bus)
    {
        _bus = bus;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {

    }
}
