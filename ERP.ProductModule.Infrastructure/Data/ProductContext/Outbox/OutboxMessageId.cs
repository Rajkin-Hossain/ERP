namespace ERP.ProductModule.Infrastructure.Data.ProductContext.Outbox;

public sealed record OutboxMessageId
{
    public Guid Value { get; init; }

    public OutboxMessageId(Guid value)
    {
        Value = value;
    }

    public static implicit operator OutboxMessageId(Guid value)
    {
        return new OutboxMessageId(value);
    }

    public static OutboxMessageId New() => new(Guid.CreateVersion7());
    public override string ToString() => Value.ToString();
}