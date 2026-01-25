using ERP.Shared.Event.Contracts.Interfaces;

namespace ERP.Shared.Event.Contracts.Modules.Products;

public sealed record ProductCreatedEvent(
    Guid ProductId,
    string Name,
    Guid CategoryId,
    string ImageUrl,
    decimal Price) : IEvent;




