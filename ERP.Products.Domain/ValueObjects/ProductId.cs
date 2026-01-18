namespace ERP.Products.Domain.ValueObjects;

public sealed record ProductId
{
    public Guid Value { get; init; }

    public ProductId(Guid value)
    {
        Value = value;
    }

    public static implicit operator ProductId(Guid value)
    {
        return new ProductId(value);
    }

    public static ProductId New() => new(Guid.CreateVersion7());
}
