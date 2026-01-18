using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.MessageOrchestrator.Contracts.Products.MessageEvents;

public record ProductCreatedMessageEvent(
    Guid ProductId,
    string Name,
    Guid CategoryId,
    string ImageUrl,
    decimal Price) : IMessageEvent;




