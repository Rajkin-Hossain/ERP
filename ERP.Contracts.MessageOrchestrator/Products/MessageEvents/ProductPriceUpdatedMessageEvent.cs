using ERP.Contracts.MessageOrchestrator.Interfaces;

namespace ERP.Contracts.MessageOrchestrator.Products.MessageEvents;

public sealed record ProductPriceUpdatedMessageEvent(
    Guid ProductId,
    decimal NewPrice) : IMessageEvent;
