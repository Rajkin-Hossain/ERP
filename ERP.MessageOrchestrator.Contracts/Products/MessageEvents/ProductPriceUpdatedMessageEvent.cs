using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.MessageOrchestrator.Contracts.Products.MessageEvents;

public sealed record ProductPriceUpdatedMessageEvent(
    Guid ProductId,
    decimal NewPrice) : IMessageEvent;
