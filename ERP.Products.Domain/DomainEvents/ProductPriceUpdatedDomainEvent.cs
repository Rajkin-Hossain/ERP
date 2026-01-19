using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Domain.Interfaces;

namespace ERP.Products.Domain.DomainEvents;

public record ProductPriceUpdatedDomainEvent(
    ProductId ProductId,
    Price Price) : IDomainEvent;
