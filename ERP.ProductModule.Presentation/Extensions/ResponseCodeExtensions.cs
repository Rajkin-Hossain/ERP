using ERP.SharedKernal;

namespace ERP.ProductModule.Presentation.Extensions;

public static class ResponseCodeExtensions
{
    public static RouteHandlerBuilder ProducesStandardApiResponses(
        this RouteHandlerBuilder builder)
    {
        return builder
            .Produces<ApiResponse<string>>(StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse<string>>(StatusCodes.Status403Forbidden)
            .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<string>>(StatusCodes.Status409Conflict)
            .Produces<ApiResponse<string>>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);
    }
}
