namespace ERP.Product.Domain.ValueObjects;

public record ProductName
{
    public string Value { get; init; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Product name cannot be empty.", nameof(value));
        if (value.Length > 200)
            throw new ArgumentException("Product name cannot exceed 200 characters.", nameof(value));
        Value = value;
    }

    public static implicit operator ProductName(string value)
    {
        return new ProductName(value);
    }
}
