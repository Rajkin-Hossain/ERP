namespace ERP.Products.Application.Result;

public sealed record AppError(AppErrorType Type, string Message);

