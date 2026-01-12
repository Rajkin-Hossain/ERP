namespace ERP.Auth.Application.Models;

public sealed record AuthChallenge(
    string Name,
    string? Session,
    string Message);
