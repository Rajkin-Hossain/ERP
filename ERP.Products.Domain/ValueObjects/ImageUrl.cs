using ERP.Shared.Domain.Exceptions;

namespace ERP.Products.Domain.ValueObjects;

public sealed record ImageUrl
{
    public string Value { get; init; }

    public ImageUrl(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Value = string.Empty;
            return;
        }

        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            throw new DomainException("Invalid image URL format.");

        Value = value;
    }

    public static ImageUrl Create(string value) => new(value);

    public static implicit operator string(ImageUrl url) => url.Value;
    public static implicit operator ImageUrl(string value) => new(value);
}
