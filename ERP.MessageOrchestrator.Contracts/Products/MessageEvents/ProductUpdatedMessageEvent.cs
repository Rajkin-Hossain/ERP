using ERP.MessageOrchestrator.Contracts.Interfaces;

namespace ERP.MessageOrchestrator.Contracts.Products.MessageEvents;

public record ProductUpdatedMessageEvent(Guid ProductId) : IMessageEvent;




