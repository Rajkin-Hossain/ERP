namespace ERP.Product.Domain.ValueObjects;

public sealed record CategoryId
{
    public Guid Value { get; init; }

    public CategoryId(Guid value)
    {
        Value = value;
    }

    public static implicit operator CategoryId(Guid value)
    {
        return new CategoryId(value);
    }
}