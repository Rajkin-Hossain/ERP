namespace ERP.ProductModule.Domain.ValueObjects.Outbox;

public sealed record ProductOutboxMessageId
{
    public Guid Value { get; init; }

    public ProductOutboxMessageId(Guid value)
    {
        Value = value;
    }

    public static implicit operator ProductOutboxMessageId(Guid value)
    {
        return new ProductOutboxMessageId(value);
    }

    public static ProductOutboxMessageId New() => new(Guid.CreateVersion7());
    public override string ToString() => Value.ToString();
}