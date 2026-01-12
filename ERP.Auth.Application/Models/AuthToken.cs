namespace ERP.Auth.Application.Models;

public sealed record AuthToken(
    string AccessToken,
    string? IdToken,
    string? RefreshToken,
    string TokenType);
