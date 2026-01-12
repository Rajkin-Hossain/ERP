namespace ERP.SharedKernal;

public sealed class DomainException(string message) : Exception(message)
{
}
