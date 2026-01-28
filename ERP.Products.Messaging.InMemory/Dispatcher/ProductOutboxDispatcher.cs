using ERP.Products.Application.Abstraction.Interfaces;
using ERP.Products.Messaging.InMemory.Mappers;
using ERP.Shared.Application.Abstractions.Outbox;
using Wolverine;

namespace ERP.Products.Messaging.InMemory.Dispatcher;

public class ProductOutboxDispatcher(
    IMessageBus bus,
    IOutboxStorage outboxStorage) : IProductOutboxDispatcher
{
    private readonly IMessageBus _bus = bus;
    private readonly IOutboxStorage _outboxStorage = outboxStorage;

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        var messages = await _outboxStorage.GetUnprocessedMessagesAsync(ct);
        var pendingMessages = messages.ToList();

        if (pendingMessages.Count == 0) return;

        // Publish all messages (LATER SEARCH FOR CONCURRENTLY PROCESS)
        foreach (var m in pendingMessages)
        {
            var messageEvent = MessageEventMapper.Map(m.EventType, m.Payload);
            await _bus.PublishAsync(messageEvent);
        }

        // Mark all as sent and save once
        foreach (var message in pendingMessages)
        {
            await _outboxStorage.MarkSent(message, ct);
        }

        await _outboxStorage.SaveChangesAsync(ct);
    }
}







