using ERP.SharedKernal.Interfaces;

namespace ERP.Product.Domain.DomainEvents;

public record ProductUpdatedEvent : IDomainEvent
{
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
