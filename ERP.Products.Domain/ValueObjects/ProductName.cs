using ERP.Shared.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record ProductName
{
    public string Value { get; init; }

    private ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Product name cannot be empty.");

        if (value.Length > 200)
            throw new DomainException("Product name cannot exceed 200 characters.");

        Value = value;
    }

    public static ProductName Create(string value) => new(value);

    public static implicit operator string(ProductName name) => name.Value;
    public static implicit operator ProductName(string value) => new(value);
}
