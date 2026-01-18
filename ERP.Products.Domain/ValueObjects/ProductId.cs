using ERP.Shared.Domain;

namespace ERP.Products.Domain.ValueObjects;

public sealed record ProductId
{
    public Guid Value { get; init; }

    public ProductId(Guid value)
    {
        Value = value;
    }

    public static DomainResult<ProductId> Create(Guid value)
    {
        if (value == Guid.Empty)
            return DomainResult<ProductId>.Failure("Product Id cannot be empty.");

        return DomainResult<ProductId>.Success(new ProductId(value));
    }

    public static implicit operator ProductId(Guid value)
    {
        return new ProductId(value);
    }

    public static ProductId New() => new(Guid.CreateVersion7());
}
