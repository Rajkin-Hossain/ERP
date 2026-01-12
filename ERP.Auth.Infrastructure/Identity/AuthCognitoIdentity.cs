using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using ERP.Auth.Application.Interfaces;
using ERP.Auth.Application.Models;
using ERP.Auth.Application.Requests;
using ERP.Auth.Application.Responses;
using ERP.Auth.Infrastructure.Options;
using ERP.SharedKernal.AppResult;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ERP.Auth.Infrastructure.Identity;

public sealed class AuthCognitoIdentity : IAuthIdentity
{
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private readonly CognitoOptions _opt;

    public AuthCognitoIdentity(IOptions<CognitoOptions> options)
    {
        _opt = options.Value;

        // You can also inject IAmazonCognitoIdentityProvider instead of new-ing it.
        _cognito = new AmazonCognitoIdentityProviderClient(
            RegionEndpoint.GetBySystemName(_opt.Region));
    }

    public async Task<AppResult<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        try
        {
            var signUp = new SignUpRequest
            {
                ClientId = _opt.ClientId,
                Username = request.Username,
                Password = request.Password,
                UserAttributes =
                [
                    new AttributeType { Name = "email", Value = request.Email }
                ]
            };

            AddSecretHashIfNeeded(signUp, request.Username);

            var resp = await _cognito.SignUpAsync(signUp, ct);

            var userConfirmed = resp.UserConfirmed == null ? false : resp.UserConfirmed;

            return AppResult<RegisterResponse>.Ok(new RegisterResponse(
                UserConfirmed: resp.UserConfirmed,
                UserSub: resp.UserSub
            ));
        }
        catch (UsernameExistsException)
        {
            return AppResult<RegisterResponse>.Fail(new AppError(ErrorType.Validation, "Username already exists."));
        }
        catch (InvalidPasswordException ex)
        {
            return AppResult<RegisterResponse>.Fail(new AppError(ErrorType.Validation, ex.Message));
        }
        catch (Exception ex)
        {
            return AppResult<RegisterResponse>.Fail(new AppError(ErrorType.Unexpected, ex.Message));
        }
    }

    public async Task<AppResult<bool>> ConfirmAsync(ConfirmRequest request, CancellationToken ct)
    {
        try
        {
            var confirm = new ConfirmSignUpRequest
            {
                ClientId = _opt.ClientId,
                Username = request.Username,
                ConfirmationCode = request.Code
            };

            AddSecretHashIfNeeded(confirm, request.Username);

            await _cognito.ConfirmSignUpAsync(confirm, ct);
            return AppResult<bool>.Ok(true);
        }
        catch (CodeMismatchException)
        {
            return AppResult<bool>.Fail(new AppError(ErrorType.Validation, "Invalid confirmation code."));
        }
        catch (ExpiredCodeException)
        {
            return AppResult<bool>.Fail(new AppError(ErrorType.Forbidden, "Confirmation code expired."));
        }
        catch (Exception ex)
        {
            return AppResult<bool>.Fail(new AppError(ErrorType.Unexpected, ex.Message));
        }
    }

    public async Task<AppResult<(AuthToken? Tokens, AuthChallenge? Challenge)>> LoginAsync(
        LoginRequest request,
        CancellationToken ct)
    {
        try
        {
            var init = new InitiateAuthRequest
            {
                ClientId = _opt.ClientId,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    ["USERNAME"] = request.Username,
                    ["PASSWORD"] = request.Password
                }
            };

            AddSecretHashIfNeeded(init, request.Username);

            var resp = await _cognito.InitiateAuthAsync(init, ct);

            // If Cognito requires a challenge (MFA / NEW_PASSWORD_REQUIRED / etc.)
            if (resp.ChallengeName is not null)
            {
                return AppResult<(AuthToken?, AuthChallenge?)>.Ok((
                    null,
                    new AuthChallenge(
                        Name: resp.ChallengeName.Value,
                        Session: resp.Session,
                        Message: "Additional challenge required."
                    )
                ));
            }

            var r = resp.AuthenticationResult;
            return AppResult<(AuthToken?, AuthChallenge?)>.Ok((
                new AuthToken(
                    AccessToken: r.AccessToken,
                    IdToken: r.IdToken,
                    RefreshToken: r.RefreshToken,
                    TokenType: r.TokenType
                ),
                null
            ));
        }
        catch (NotAuthorizedException)
        {
            return AppResult<(AuthToken?, AuthChallenge?)>.Fail(new AppError(ErrorType.Validation, "Invalid username or password."));
        }
        catch (UserNotConfirmedException)
        {
            return AppResult<(AuthToken?, AuthChallenge?)>.Fail(new AppError(ErrorType.Validation, "User is not confirmed."));
        }
        catch (Exception ex)
        {
            return AppResult<(AuthToken?, AuthChallenge?)>.Fail(new AppError(ErrorType.Unexpected, ex.Message));
        }
    }

    public async Task<AppResult<AuthToken>> RefreshAsync(RefreshRequest request, CancellationToken ct)
    {
        try
        {
            var init = new InitiateAuthRequest
            {
                ClientId = _opt.ClientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    ["REFRESH_TOKEN"] = request.RefreshToken
                }
            };

            // If client secret exists, Cognito usually expects SECRET_HASH computed with USERNAME + ClientId.
            // But refresh flow doesn't include username; you must supply it if you want to refresh in your API.
            if (!string.IsNullOrWhiteSpace(_opt.ClientSecret))
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return AppResult<AuthToken>.Fail(new AppError(
                        ErrorType.Validation,
                        "Username is required for refresh when app client uses a client secret."
                    ));
                }

                init.AuthParameters["SECRET_HASH"] = ComputeSecretHash(_opt.ClientId, _opt.ClientSecret!, request.Username);
                init.AuthParameters["USERNAME"] = request.Username; // safe to include
            }

            var resp = await _cognito.InitiateAuthAsync(init, ct);
            var r = resp.AuthenticationResult;

            return AppResult<AuthToken>.Ok(new AuthToken(
                AccessToken: r.AccessToken,
                IdToken: r.IdToken,
                RefreshToken: r.RefreshToken,
                TokenType: r.TokenType
            ));
        }
        catch (NotAuthorizedException)
        {
            return AppResult<AuthToken>.Fail(new AppError(ErrorType.Forbidden, "Refresh token is invalid or expired."));
        }
        catch (Exception ex)
        {
            return AppResult<AuthToken>.Fail(new AppError(ErrorType.Unexpected, ex.Message));
        }
    }

    // -------- helpers --------

    private void AddSecretHashIfNeeded(SignUpRequest req, string username)
    {
        if (string.IsNullOrWhiteSpace(_opt.ClientSecret)) return;
        req.SecretHash = ComputeSecretHash(_opt.ClientId, _opt.ClientSecret!, username);
    }

    private void AddSecretHashIfNeeded(ConfirmSignUpRequest req, string username)
    {
        if (string.IsNullOrWhiteSpace(_opt.ClientSecret)) return;
        req.SecretHash = ComputeSecretHash(_opt.ClientId, _opt.ClientSecret!, username);
    }

    private void AddSecretHashIfNeeded(InitiateAuthRequest req, string username)
    {
        if (string.IsNullOrWhiteSpace(_opt.ClientSecret)) return;
        req.AuthParameters["SECRET_HASH"] = ComputeSecretHash(_opt.ClientId, _opt.ClientSecret!, username);
    }

    private static string ComputeSecretHash(string clientId, string clientSecret, string username)
    {
        var data = Encoding.UTF8.GetBytes(username + clientId);
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret));
        return Convert.ToBase64String(hmac.ComputeHash(data));
    }
}
