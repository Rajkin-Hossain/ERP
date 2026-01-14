using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.Domain.ValueObjects.Outbox;
using ERP.SharedKernal.Entities;
using ERP.SharedKernal.Enums;
using ERP.SharedKernal.Interfaces;
using System.Text.Json;

namespace ERP.ProductModule.Domain.Entities.Outbox;

public sealed class ProductOutboxMessage : Entity<ProductOutboxMessageId>
{
    private ProductOutboxMessage() { } // EF

    public string EventType { get; private set; } = default!;
    public string AggregateId { get; private set; } = default!; // store as string
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? PublishedOnUtc { get; private set; }
    public string Payload { get; private set; } = default!;
    public OutboxStatus Status { get; private set; }
    public int RetryCount { get; private set; }

    public static ProductOutboxMessage Create(
        ProductId aggregateId,
        IDomainEvent domainEvent,
        JsonSerializerOptions? jsonOptions = null)
    {
        var eventType = domainEvent.GetType();

        return new ProductOutboxMessage
        {
            Id = ProductOutboxMessageId.New(),
            EventType = eventType.FullName ?? eventType.Name,
            AggregateId = aggregateId.ToString()!, // VO ids should override ToString()
            OccurredOnUtc = DateTime.UtcNow,
            Payload = JsonSerializer.Serialize(domainEvent, eventType, jsonOptions),
            Status = OutboxStatus.Pending,
            RetryCount = 0
        };
    }

    public void MarkSent(DateTime publishedOnUtc)
    {
        Status = OutboxStatus.Sent;
        PublishedOnUtc = publishedOnUtc;
    }

    public void MarkFailed(DateTime publishedOnUtc)
    {
        Status = OutboxStatus.Failed;
        PublishedOnUtc = publishedOnUtc;
    }

    public void IncrementRetry()
    {
        RetryCount++;
    }
}