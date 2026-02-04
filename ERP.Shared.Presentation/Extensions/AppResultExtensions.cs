using ERP.Shared.Application.AppRecords;
using ERP.Shared.Presentation.Models;
using Microsoft.AspNetCore.Http;

namespace ERP.Shared.Presentation.Extensions;

public static class AppResultExtensions
{
    public static IResult ToApiResponse<T>(this AppResult<T> result)
    {
        if (result.IsSuccess)
        {
            return TypedResults.Ok(ApiResponse<T>.Ok(result.Value!));
        }

        if (result.Errors is null || result.Errors.Count == 0)
        {
            return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
        }

        var dominantType = ResolveAppErrorType(result.Errors.Select(e => e.Type));

        var relevantErrors = result.Errors
            .Where(e => e.Type == dominantType)
            .ToArray();

        var messages = relevantErrors.Select(e => e.Message).ToArray();

        return dominantType switch
        {
            AppErrorType.Validation => TypedResults.BadRequest(
                ApiResponse<string>.Fail(
                    message: "Validation failed.",
                    errors: messages)),

            AppErrorType.Conflict => TypedResults.Conflict(
                ApiResponse<string>.Fail(
                    message: "Conflict occurred.",
                    errors: messages)),

            AppErrorType.NotFound => TypedResults.NotFound(
                ApiResponse<string>.Fail(
                    message: "Resource not found.",
                    errors: messages)),

            AppErrorType.Unauthorized => TypedResults.Unauthorized(),

            AppErrorType.Forbidden => TypedResults.Forbid(),

            _ => TypedResults.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    private static AppErrorType ResolveAppErrorType(IEnumerable<AppErrorType> types)
    {
        if (types.Contains(AppErrorType.Unauthorized)) return AppErrorType.Unauthorized;
        if (types.Contains(AppErrorType.Forbidden)) return AppErrorType.Forbidden;
        if (types.Contains(AppErrorType.NotFound)) return AppErrorType.NotFound;
        if (types.Contains(AppErrorType.Conflict)) return AppErrorType.Conflict;
        if (types.Contains(AppErrorType.Validation)) return AppErrorType.Validation;

        return AppErrorType.Unexpected;
    }
}




