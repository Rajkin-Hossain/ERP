using BuildingBlocks.Domain.Interfaces;
using ERP.Products.Domain.ValueObjects;
using ERP.Products.Domain.Enums;
using System.Text.Json;

namespace ERP.Products.Domain.Entities;

public sealed class ProductOutboxMessage
{
    private ProductOutboxMessage() { }

    public Guid Id { get; private set; }
    public string EventType { get; private set; } = default!;
    public Guid AggregateId { get; private set; } = default!;
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
            Id = Guid.NewGuid(),
            EventType = eventType.FullName ?? eventType.Name,
            AggregateId = aggregateId.Value,
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
