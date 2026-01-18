using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.MessageOrchestrator.Interfaces;

public interface IServiceBus
{
    Task PublishCommandAsync<T>(T message, CancellationToken ct = default) where T : IMessageCommand;
}

