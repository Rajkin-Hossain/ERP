using BuildingBlocks.Application.Interfaces;

namespace ERP.Products.Messaging.RabbitMQ.Outbox;

public class ProductOutboxJob
{
    private readonly IServiceBus _bus;

    public ProductOutboxJob(IServiceBus bus)
    {
        _bus = bus;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {

    }
}
