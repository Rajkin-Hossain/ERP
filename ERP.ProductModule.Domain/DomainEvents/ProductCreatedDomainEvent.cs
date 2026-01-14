using ERP.SharedKernal.Interfaces;

namespace ERP.ProductModule.Domain.DomainEvents;

public record ProductCreatedDomainEvent(Guid ProductId) : IDomainEvent { }
