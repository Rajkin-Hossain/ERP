using ERP.Shared.Domain;

namespace ERP.Products.Domain.ValueObjects;

public record CategoryName
{
    public string Value { get; init; }

    public CategoryName(string value)
    {
        Value = value;
    }

    public static DomainResult<CategoryName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainResult<CategoryName>.Failure("Category name cannot be empty.");
            
        if (value.Length > 200)
            return DomainResult<CategoryName>.Failure("Category name cannot exceed 200 characters.");

        return DomainResult<CategoryName>.Success(new CategoryName(value));
    }

    public static implicit operator string(CategoryName name) => name.Value;
}
