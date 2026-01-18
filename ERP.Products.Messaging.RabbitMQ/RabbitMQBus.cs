using ERP.Products.Messaging.RabbitMQ.Interfaces;
using MassTransit;
namespace ERP.Products.Messaging.RabbitMQ;

public class RabbitMQBus : IServiceBus
{
    private readonly IBus _bus;

    public RabbitMQBus(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
    {
        await _bus.Publish(message, ct);
    }
}



