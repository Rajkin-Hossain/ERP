namespace ERP.Auth.Infrastructure.Options;

public sealed class CognitoOptions
{
    public string Region { get; init; } = default!;
    public string UserPoolId { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string? ClientSecret { get; init; } // null/empty for public clients
}
