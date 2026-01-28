namespace ERP.Shared.Application.Abstractions.Outbox;

public sealed record OutboxEnvelope
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = default!;
    public string AggregateId { get; set; } = default!;
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
    public string Payload { get; set; } = default!;
    public int Status { get; set; }
    public int RetryCount { get; set; }
}
