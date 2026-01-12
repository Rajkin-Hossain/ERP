namespace ERP.SharedKernal.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
