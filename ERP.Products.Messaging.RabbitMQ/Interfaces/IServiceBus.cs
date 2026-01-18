using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.Products.Messaging.RabbitMQ.Interfaces;

public interface IServiceBus
{
    Task PublishEventAsync<T>(T message, CancellationToken ct = default) where T : IMessageEvent;
}




