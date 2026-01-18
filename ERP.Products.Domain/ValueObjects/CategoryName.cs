using ERP.Shared.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public record CategoryName
{
    public string Value { get; init; }

    public CategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Category name cannot be empty.");
            
        if (value.Length > 200)
            throw new DomainException("Category name cannot exceed 200 characters.");

        Value = value;
    }

    public static implicit operator string(CategoryName name) => name.Value;
    public static implicit operator CategoryName(string value) => new(value);
}
