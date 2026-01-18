using ERP.Shared.Domain;

namespace ERP.Products.Domain.ValueObjects;

public sealed record ImageUrl
{
    public string Value { get; init; }

    public ImageUrl(string value)
    {
        Value = value;
    }

    public static DomainResult<ImageUrl> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainResult<ImageUrl>.Success(new ImageUrl(value));

        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            return DomainResult<ImageUrl>.Failure("Invalid image URL format.");

        return DomainResult<ImageUrl>.Success(new ImageUrl(value));
    }

    public static implicit operator string(ImageUrl url) => url.Value;

    public static implicit operator ImageUrl(string url) => new(url);
}
