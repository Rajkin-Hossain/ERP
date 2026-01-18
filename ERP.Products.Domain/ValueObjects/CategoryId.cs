using ERP.Shared.Domain;

namespace ERP.Products.Domain.ValueObjects;

public sealed record CategoryId
{
    public Guid Value { get; init; }

    public CategoryId(Guid value)
    {
        Value = value;
    }

    public static DomainResult<CategoryId> Create(Guid value)
    {
        if (value == Guid.Empty)
            return DomainResult<CategoryId>.Failure("Category Id cannot be empty.");

        return DomainResult<CategoryId>.Success(new CategoryId(value));
    }

    public static implicit operator CategoryId(Guid value)
    {
        return new CategoryId(value);
    }

    public static CategoryId New() => new(Guid.CreateVersion7());
}
