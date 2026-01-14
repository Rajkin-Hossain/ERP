using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.Orchestrator.Contract.ProductModule.Events;

public record ProductCreatedEvent(Guid ProductId) : IEvent;
