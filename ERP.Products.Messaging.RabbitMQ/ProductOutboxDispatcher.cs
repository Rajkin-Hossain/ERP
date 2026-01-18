using ERP.Products.Application.Interfaces;
using ERP.Products.Messaging.RabbitMQ.Interfaces;
namespace ERP.Products.Messaging.RabbitMQ;

public class ProductOutboxDispatcher : IProductOutboxDispatcher
{
    private readonly IServiceBus _bus;
    private readonly IOutboxRepository _outboxRepository;

    public ProductOutboxDispatcher(IServiceBus bus, IOutboxRepository outboxRepository)
    {
        _bus = bus;
        _outboxRepository = outboxRepository;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        var messages = await _outboxRepository.GetUnprocessedMessagesAsync(ct);
        var pendingMessages = messages.ToList();

        if (pendingMessages.Count == 0) return;

        // Publish all messages in parallel
        await Task.WhenAll(pendingMessages.Select(m =>
        {
            var messageEvent = IntegrationEventMapper.Map(m.EventType, m.Payload);
            return _bus.PublishEventAsync(messageEvent, ct);
        }));

        // Mark all as sent and save once
        foreach (var message in pendingMessages)
        {
            await _outboxRepository.MarkSent(message, ct);
        }

        await _outboxRepository.SaveChangesAsync(ct);
    }
}







