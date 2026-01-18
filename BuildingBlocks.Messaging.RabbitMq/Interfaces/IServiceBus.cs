namespace BuildingBlocks.Application.Interfaces;

public interface IServiceBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}
