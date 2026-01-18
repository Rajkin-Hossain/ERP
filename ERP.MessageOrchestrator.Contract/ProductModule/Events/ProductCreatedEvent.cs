using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.ProductModule.Events;

public record ProductCreatedEvent(Guid ProductId) : IEvent;
