namespace ERP.SharedKernal.Exceptions;

public sealed class DomainException(string message) : Exception(message)
{
}
