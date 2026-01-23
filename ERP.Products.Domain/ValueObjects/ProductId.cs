using ERP.Shared.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record ProductId
{
    public Guid Value { get; init; }

    private ProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Product Id cannot be empty.");

        Value = value;
    }

    public ProductId(string value)
    {
        if (!Guid.TryParse(value, out var guid))
            throw new DomainException("Invalid Product Id format.");

        if (guid == Guid.Empty)
            throw new DomainException("Product Id cannot be empty.");

        Value = guid;
    }

    public static ProductId Create(Guid value) => new(value);

    public static implicit operator ProductId(Guid value) => new(value);
    public static implicit operator ProductId(string value) => new(value);
    public static implicit operator Guid(ProductId id) => id.Value;

    public static ProductId New() => new(Guid.CreateVersion7());
}
