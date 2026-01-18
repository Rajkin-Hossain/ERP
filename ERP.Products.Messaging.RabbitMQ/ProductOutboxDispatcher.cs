using ERP.Products.Application.Interfaces;
using ERP.Products.Messaging.RabbitMQ.Interfaces;
namespace ERP.Products.Messaging.RabbitMQ;

public class ProductOutboxDispatcher : IProductOutboxDispatcher
{
    private readonly IServiceBus _bus;
    private readonly IOutboxRepository _outboxRepository;

    public ProductOutboxDispatcher(IServiceBus bus, IOutboxRepository outboxRepository)
    {
        _bus = bus;
        _outboxRepository = outboxRepository;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
    }
}







