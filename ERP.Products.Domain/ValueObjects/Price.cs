using ERP.Shared.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record Price
{
    public decimal Value { get; }

    private Price(decimal value)
    {
        if (value < 0)
            throw new DomainException("Price cannot be negative.");

        Value = value;
    }

    public static Price Create(decimal value) => new(value);

    public static implicit operator decimal(Price price) => price.Value;
    public static implicit operator Price(decimal value) => new(value);
}
