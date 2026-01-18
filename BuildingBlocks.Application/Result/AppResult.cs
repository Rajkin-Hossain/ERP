namespace BuildingBlocks.Application.Result;

public sealed class AppResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IReadOnlyList<AppError> Errors { get; }

    private AppResult(bool isSuccess, T? value, IReadOnlyList<AppError> errors)
        => (IsSuccess, Value, Errors) = (isSuccess, value, errors);

    public static AppResult<T> Ok(T value) => new(true, value, []);
    public static AppResult<T> Fail(params AppError[] errors) => new(false, default, errors);
}
