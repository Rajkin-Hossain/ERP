using ERP.Products.Messaging.InMemory.Mappers;
using ERP.Shared.Application.Abstractions.Outbox;
using ERP.Shared.Event.Contracts.Interfaces;
using Wolverine;

namespace ERP.Products.Messaging.InMemory.Dispatcher;

public class ProductOutboxDispatcher(IMessageBus bus, IOutboxStore outboxRepository) : IProductOutboxDispatcher
{
    private readonly IMessageBus _bus = bus;
    private readonly IOutboxStore _outboxRepository = outboxRepository;

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        IEnumerable<OutboxMessage> messages = await _outboxRepository.GetUnprocessedMessagesAsync(ct);
        List<OutboxMessage> pendingMessages = messages.ToList();

        if (pendingMessages.Count == 0) return;

        // Publish all messages (LATER SEARCH FOR CONCURRENTLY PROCESS)
        foreach (OutboxMessage? m in pendingMessages)
        {
            IEvent messageEvent = MessageEventMapper.Map(m.EventType, m.Payload);

            //Events always published from a BC so that, its QueryHandler, Saga, etc can subscribe to it.
            //QueryHandler receives and update new data to read DB.
            await _bus.PublishAsync(messageEvent);
        }

        // Mark all as sent and save once
        foreach (OutboxMessage? message in pendingMessages)
        {
            await _outboxRepository.MarkSent(message, ct);
        }

        await _outboxRepository.SaveChangesAsync(ct);
    }
}







