using ERP.SharedKernal.Interfaces;

namespace ERP.ProductModule.Domain.DomainEvents;

public record ProductUpdatedEvent : IDomainEvent
{
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
