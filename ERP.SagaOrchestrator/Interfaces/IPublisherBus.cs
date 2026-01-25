namespace ERP.SagaOrchestrator.Interfaces;

public interface IPublisherBus
{
    Task PublishCommandAsync<T>(T message, CancellationToken ct = default) where T : ICommand;
}

