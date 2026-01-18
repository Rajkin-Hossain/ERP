using BuildingBlocks.Application.Interfaces;
using MassTransit;

namespace BuildingBlocks.Messaging.RabbitMq;

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