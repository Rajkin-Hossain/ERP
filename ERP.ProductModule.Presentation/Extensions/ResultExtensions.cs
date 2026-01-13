using ERP.SharedKernal;
using ERP.SharedKernal.AppResult;

namespace ERP.ProductModule.Presentation.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this AppResult<T> result)
    {
        if (result.IsSuccess)
        {
            return TypedResults.Ok(ApiResponse<T>.Ok(result.Value!));
        }

        if (result.Errors is null || result.Errors.Count == 0)
        {
            return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
        }

        var dominantType = ResolveErrorType(result.Errors.Select(e => e.Type));

        var relevantErrors = result.Errors
            .Where(e => e.Type == dominantType)
            .ToArray();

        var messages = relevantErrors.Select(e => e.Message).ToArray();

        return dominantType switch
        {
            ErrorType.Validation => TypedResults.BadRequest(
                ApiResponse<string>.Fail(
                    message: "Validation failed.",
                    errors: messages)),

            ErrorType.Conflict => TypedResults.Conflict(
                ApiResponse<string>.Fail(
                    message: "Conflict occurred.",
                    errors: messages)),

            ErrorType.NotFound => TypedResults.NotFound(
                ApiResponse<string>.Fail(
                    message: "Resource not found.",
                    errors: messages)),

            ErrorType.Unauthorized => TypedResults.Unauthorized(),

            ErrorType.Forbidden => TypedResults.Forbid(),

            _ => TypedResults.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    private static ErrorType ResolveErrorType(IEnumerable<ErrorType> types)
    {
        if (types.Contains(ErrorType.Unauthorized)) return ErrorType.Unauthorized;
        if (types.Contains(ErrorType.Forbidden)) return ErrorType.Forbidden;
        if (types.Contains(ErrorType.NotFound)) return ErrorType.NotFound;
        if (types.Contains(ErrorType.Conflict)) return ErrorType.Conflict;
        if (types.Contains(ErrorType.Validation)) return ErrorType.Validation;

        return ErrorType.Unexpected;
    }
}
