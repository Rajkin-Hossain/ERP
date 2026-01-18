using ERP.MessageOrchestrator.Contracts.Interfaces;
using ERP.MessageOrchestrator.Interfaces;
using MassTransit;

namespace ERP.MessageOrchestrator;

public class RabbitMQBus : IServiceBus
{
    private readonly IBus _bus;

    public RabbitMQBus(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishCommandAsync<T>(T message, CancellationToken ct = default) where T : IMessageCommand
    {
        await _bus.Publish(message, ct);
    }
}
