namespace ERP.MessageOrchestrator.Interfaces;

public interface IServiceBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}

