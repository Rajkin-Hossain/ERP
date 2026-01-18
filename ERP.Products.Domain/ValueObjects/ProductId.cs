using ERP.Shared.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record ProductId
{
    public Guid Value { get; init; }

    public ProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Product Id cannot be empty.");

        Value = value;
    }

    public static ProductId Create(Guid value) => new(value);

    public static implicit operator ProductId(Guid value) => new(value);
    public static implicit operator Guid(ProductId id) => id.Value;

    public static ProductId New() => new(Guid.CreateVersion7());
}
