using ERP.Shared.Presentation.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ERP.Shared.Presentation.Extensions;

public static class RouteBuilderExtensions
{
    public static RouteHandlerBuilder ProducesStandardApiResponses(
        this RouteHandlerBuilder builder)
    {
        return builder
            // Success
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status201Created)

            // Client errors
            .Produces<ApiResponse<string>>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse<string>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<string>>(StatusCodes.Status403Forbidden)
            .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<string>>(StatusCodes.Status409Conflict)

            // Server error
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);
    }

    public static RouteHandlerBuilder ProducesStandardReadApiResponses(
        this RouteHandlerBuilder builder)
    {
        return builder
            // Success
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)

            // Client errors
            .Produces<ApiResponse<string>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<string>>(StatusCodes.Status403Forbidden)
            .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)

            // Server error
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);
    }
}
