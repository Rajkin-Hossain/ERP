using ERP.Auth.Application.Interfaces;
using ERP.Auth.Application.Models;
using ERP.Auth.Application.Requests;
using ERP.Auth.Application.Responses;
using ERP.SharedKernal.AppResult;

namespace ERP.Auth.Application.UserCases;

public sealed class AuthService(IAuthIdentity identity)
{
    private readonly IAuthIdentity _identity = identity;

    public Task<AppResult<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct)
        => _identity.RegisterAsync(request, ct);

    public Task<AppResult<bool>> ConfirmAsync(ConfirmRequest request, CancellationToken ct)
        => _identity.ConfirmAsync(request, ct);

    public Task<AppResult<(AuthToken? Tokens, AuthChallenge? Challenge)>> LoginAsync(LoginRequest request, CancellationToken ct)
        => _identity.LoginAsync(request, ct);

    public Task<AppResult<AuthToken>> RefreshAsync(RefreshRequest request, CancellationToken ct)
        => _identity.RefreshAsync(request, ct);
}
