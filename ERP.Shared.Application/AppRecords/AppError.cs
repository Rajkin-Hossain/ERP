namespace ERP.Shared.Application.AppRecords;

public sealed record AppError(AppErrorType Type, string Message);

public enum AppErrorType
{
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict,
    Validation,
    Unexpected
}


