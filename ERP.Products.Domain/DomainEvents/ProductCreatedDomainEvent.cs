using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Domain.Interfaces;


namespace ERP.Products.Domain.DomainEvents;

public record ProductCreatedDomainEvent(
    ProductId ProductId,
    ProductName ProductName,
    CategoryId CategoryId,
    ImageUrl ImageUrl,
    Price Price) : IDomainEvent;






