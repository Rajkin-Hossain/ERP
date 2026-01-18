using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.Products.MessageEvents;

public record ProductCreatedMessageEvent(Guid ProductId) : IMessageEvent;

