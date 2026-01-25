using ERP.SagaOrchestrator.Interfaces;
using MassTransit;

namespace ERP.SagaOrchestrator;

public class RabbitMQBus : IPublisherBus
{
    private readonly IBus _bus;

    public RabbitMQBus(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishCommandAsync<T>(T message, CancellationToken ct = default) where T : ICommand
    {
        await _bus.Publish(message, ct);
    }
}
