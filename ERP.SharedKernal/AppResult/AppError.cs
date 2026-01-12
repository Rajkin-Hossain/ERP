namespace ERP.SharedKernal.AppResult;

/*Sample:  AppError(ErrorType.Conflict, "discussion.depth_exceeded", "Maximum discussion depth is 2."));*/
public sealed record AppError(ErrorType Type, string Message);
