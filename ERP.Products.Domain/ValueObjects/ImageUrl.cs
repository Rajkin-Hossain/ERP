using BuildingBlocks.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record ImageUrl
{
    public string Value { get; init; }

    public ImageUrl(string value)
    {
        Value = value;

        if (string.IsNullOrWhiteSpace(value))
            return; // allow empty = no image (optional)

        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            throw new DomainException("Invalid image URL.");
    }

    public static implicit operator ImageUrl(string value)
    {
        return new ImageUrl(value);
    }
}