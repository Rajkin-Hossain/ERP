using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.Orchestrator.Contract.Products.MessageEvents;

public record ProductCreatedMessageEvent(Guid ProductId) : IMessageEvent;

