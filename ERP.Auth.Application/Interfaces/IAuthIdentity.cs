using ERP.Auth.Application.Models;
using ERP.Auth.Application.Requests;
using ERP.Auth.Application.Responses;
using ERP.SharedKernal.AppResult;

namespace ERP.Auth.Application.Interfaces;

public interface IAuthIdentity
{
    Task<AppResult<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<AppResult<bool>> ConfirmAsync(ConfirmRequest request, CancellationToken ct);

    /// <summary>
    /// Returns tokens on success, or a challenge if the user needs MFA/new password/etc.
    /// </summary>
    Task<AppResult<(AuthToken? Tokens, AuthChallenge? Challenge)>> LoginAsync(LoginRequest request, CancellationToken ct);

    /// <summary>
    /// Refreshes access/id token (refresh token is usually not re-issued).
    /// If your app client has a secret, you typically need Username to compute SECRET_HASH.
    /// </summary>
    Task<AppResult<AuthToken>> RefreshAsync(RefreshRequest request, CancellationToken ct);
}