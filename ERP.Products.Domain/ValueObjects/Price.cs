using ERP.Products.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record Price
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value < 0)
            throw new DomainException("Amount cannot be negative.");

        Value = value;
    }

    public static implicit operator Price(decimal value)
    {
        return new Price(value);
    }
}

