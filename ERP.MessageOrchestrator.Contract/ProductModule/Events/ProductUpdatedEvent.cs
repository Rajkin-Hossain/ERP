using ERP.MessageOrchestrator.Contract.Interfaces;

namespace ERP.MessageOrchestrator.Contract.ProductModule.Events;

public record ProductUpdatedEvent(Guid ProductId) : IEvent;
