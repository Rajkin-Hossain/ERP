using ERP.Auth.Presentation.EndPoints.Auth.ApiGroups;

namespace ERP.Auth.Presentation.EndPoints.Auth;

public static class AuthApiEndPoints
{
    public static void MapAuthApiEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .RequireAuthorization("EmailVerified")
            .WithTags("Auth");

        group.MapAuthApiGroups();
    }
}