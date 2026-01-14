using ERP.SharedKernal.Interfaces;

namespace ERP.ProductModule.Domain.DomainEvents;

public record ProductUpdatedDomainEvent : IDomainEvent
{
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
