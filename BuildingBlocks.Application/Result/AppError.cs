namespace BuildingBlocks.Application.Result;

public sealed record AppError(AppErrorType Type, string Message);
