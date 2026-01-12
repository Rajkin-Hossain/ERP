using ERP.SharedKernal.Interfaces;

namespace ERP.Product.Domain.DomainEvents;

public sealed  record ProductCreatedEvent : IDomainEvent
{
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
