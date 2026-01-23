using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Domain.Interfaces;

namespace ERP.Products.Domain.DomainEvents;

public record ProductUpdatedDomainEvent(
    ProductId ProductId,
    ProductName ProductName,
    CategoryId CategoryId,
    ImageUrl ImageUrl,
    Price Price) : IDomainEvent;






