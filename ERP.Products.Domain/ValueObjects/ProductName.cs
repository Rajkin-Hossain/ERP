using ERP.Shared.Domain;

namespace ERP.Products.Domain.ValueObjects;

public record ProductName
{
    public string Value { get; init; }

    public ProductName(string value)
    {
        Value = value;
    }

    public static DomainResult<ProductName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainResult<ProductName>.Failure("Product name cannot be empty.");
        
        if (value.Length > 200)
            return DomainResult<ProductName>.Failure("Product name cannot exceed 200 characters.");

        return DomainResult<ProductName>.Success(new ProductName(value));
    }

    public static implicit operator string(ProductName name) => name.Value;
}


