using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.SharedKernal.Interfaces;

public interface IServiceBus
{
    Task PublishEvent(IEvent evt, CancellationToken ct);
    Task PublishCommand(ICommand command, CancellationToken ct);
}
