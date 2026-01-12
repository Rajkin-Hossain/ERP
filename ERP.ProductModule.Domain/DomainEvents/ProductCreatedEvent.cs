using ERP.SharedKernal.Interfaces;

namespace ERP.ProductModule.Domain.DomainEvents;

public sealed record ProductCreatedEvent : IDomainEvent
{
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
