using ERP.Shared.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record CategoryId
{
    public Guid Value { get; init; }

    private CategoryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Category Id cannot be empty.");

        Value = value;
    }

    private CategoryId(string value)
    {
        if (!Guid.TryParse(value, out var guid))
            throw new DomainException("Invalid Category Id format.");

        if (guid == Guid.Empty)
            throw new DomainException("Category Id cannot be empty.");

        Value = guid;
    }

    public static CategoryId Create(Guid value) => new(value);

    public static implicit operator CategoryId(Guid value) => new(value);
    public static implicit operator CategoryId(string value) => new(value);
    public static implicit operator Guid(CategoryId id) => id.Value;

    public static CategoryId New() => new(Guid.CreateVersion7());
}
