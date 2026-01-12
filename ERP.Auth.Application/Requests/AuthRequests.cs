namespace ERP.Auth.Application.Requests;

public sealed record RegisterRequest(
    string Username,
    string Password,
    string Email);

public sealed record ConfirmRequest(
    string Username,
    string Code);

public sealed record LoginRequest(
    string Username,
    string Password);

public sealed record RefreshRequest(
    string RefreshToken,
    string? Username = null); // only needed if your app client has a secret

