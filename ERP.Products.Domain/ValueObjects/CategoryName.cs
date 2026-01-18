namespace ERP.Products.Domain.ValueObjects;

public record CategoryName
{
    public string Value { get; init; }

    public CategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Category name cannot be empty.", nameof(value));
        if (value.Length > 200)
            throw new ArgumentException("Category name cannot exceed 200 characters.", nameof(value));
        Value = value;
    }

    public static implicit operator CategoryName(string value)
    {
        return new CategoryName(value);
    }
}


