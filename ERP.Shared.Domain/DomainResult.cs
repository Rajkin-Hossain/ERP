namespace ERP.Shared.Domain;

public sealed class DomainResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private DomainResult(bool isSuccess, T? value, string? errorMessage)
        => (IsSuccess, Value, ErrorMessage) = (isSuccess, value, errorMessage);

    public static DomainResult<T> Success(T value) => new(true, value, null);
    public static DomainResult<T> Failure(string errorMessage) => new(false, default, errorMessage);

    public static implicit operator bool(DomainResult<T> result) => result.IsSuccess;
}
