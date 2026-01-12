using ERP.SharedKernal;

namespace ERP.Product.Domain.ValueObjects;

public sealed record Price
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Price(decimal amount, string currency)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        Amount = amount;
        Currency = currency;
    }
}
