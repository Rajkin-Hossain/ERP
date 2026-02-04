namespace ERP.Products.Persistance.PgSQL.Outbox.Models;

public sealed class OutboxMessage
{
    private OutboxMessage() { }

    public Guid Id { get; private set; }
    public string EventType { get; private set; } = default!;
    public string AggregateId { get; private set; } = default!;
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? PublishedOnUtc { get; private set; }
    public string Payload { get; private set; } = default!;
    public OutboxStatus Status { get; private set; }
    public int RetryCount { get; private set; }

    public static OutboxMessage Create(
        string aggregateId,
        string eventType,
        string payload)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = eventType,
            AggregateId = aggregateId,
            OccurredOnUtc = DateTime.UtcNow,
            Payload = payload,
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

public enum OutboxStatus
{
    Pending = 1,
    Sent = 2,
    Failed = 3
}
