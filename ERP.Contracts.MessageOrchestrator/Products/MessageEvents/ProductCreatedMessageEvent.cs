using ERP.Contracts.MessageOrchestrator.Interfaces;

namespace ERP.Contracts.MessageOrchestrator.Products.MessageEvents;

public record ProductCreatedMessageEvent(
    Guid ProductId,
    string Name,
    Guid CategoryId,
    string ImageUrl,
    decimal Price) : IMessageEvent;




