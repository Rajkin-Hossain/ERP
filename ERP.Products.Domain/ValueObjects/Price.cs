using ERP.Shared.Domain;

namespace ERP.Products.Domain.ValueObjects;

public sealed record Price
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        Value = value;
    }

    public static DomainResult<Price> Create(decimal value)
    {
        if (value < 0)
            return DomainResult<Price>.Failure("Price cannot be negative.");

        return DomainResult<Price>.Success(new Price(value));
    }

    public static implicit operator decimal(Price price) => price.Value;
}
