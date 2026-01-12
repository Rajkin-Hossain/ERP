namespace ERP.Auth.Application.Responses;

public sealed record RegisterResponse(
    bool? UserConfirmed,
    string UserSub);