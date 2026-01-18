namespace ERP.Products.Messaging.RabbitMQ.Interfaces;

public interface IServiceBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}

