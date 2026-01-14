using ERP.Orchestrator.Contract.Interfaces;
using ERP.SharedKernal.Interfaces;
using MassTransit;

namespace ERP.ProductModule.Infrastructure.RabbitMQ;

public class RabbitMQBus : IServiceBus
{
    private readonly IBus _bus;

    public RabbitMQBus(IBus bus)
    {
        _bus = bus;
    }

    public async Task Publish(IEvent evt, CancellationToken ct = default)
    {
        await _bus.Publish(evt, evt.GetType(), ct);
    }

    public async Task Send(ICommand command, CancellationToken ct = default)
    {
        await _bus.Publish(command, command.GetType(), ct);
    }
}
