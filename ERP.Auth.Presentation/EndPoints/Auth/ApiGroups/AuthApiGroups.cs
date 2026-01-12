using ERP.Auth.Application.Requests;
using ERP.Auth.Application.UserCases;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ERP.Auth.Presentation.EndPoints.Auth.ApiGroups;

public static class AuthApiGroups
{
    public static void MapAuthApiGroups(this RouteGroupBuilder group)
    {
        group.MapPost("/auth/register", async (RegisterRequest req, AuthService auth, CancellationToken ct) =>
        {
            var result = await auth.RegisterAsync(req, ct);
            return Results.Ok(result);
        });

        group.MapPost("/auth/confirm", async (ConfirmRequest req, AuthService auth, CancellationToken ct) =>
        {
            await auth.ConfirmAsync(req, ct);
            return Results.Ok(new { ok = true });
        });

        group.MapPost("/auth/login", async (LoginRequest req, AuthService auth, CancellationToken ct) =>
        {
            var tokens = await auth.LoginAsync(req, ct);
            return Results.Ok(tokens);
        });

        group.MapPost("/auth/refresh", async (RefreshRequest req, AuthService auth, CancellationToken ct) =>
        {
            var tokens = await auth.RefreshAsync(req, ct);
            return Results.Ok(tokens);
        });

        // Example protected endpoint (requires Bearer token)
        group.MapGet("/me", [Authorize] (ClaimsPrincipal user) =>
        {
            var sub = user.FindFirst("sub")?.Value;
            var username = user.FindFirst("cognito:username")?.Value ?? user.Identity?.Name;
            var groups = user.FindAll("cognito:groups").Select(x => x.Value).ToArray();
            return Results.Ok(new { sub, username, groups });
        });
    }
}
