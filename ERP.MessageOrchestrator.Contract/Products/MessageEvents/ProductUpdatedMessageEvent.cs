using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.Products.MessageEvents;

public record ProductUpdatedMessageEvent(Guid ProductId) : IMessageEvent;

