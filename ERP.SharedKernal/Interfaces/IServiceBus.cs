using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.SharedKernal.Interfaces;

public interface IServiceBus
{
    Task Publish(IEvent evt, CancellationToken ct);
    Task Send(ICommand consumer, CancellationToken ct);
}
