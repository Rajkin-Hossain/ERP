using ERP.Orchestrator.Contract.Interfaces;

namespace ERP.Orchestrator.Contract.ProductModule.Events;

public record ProductUpdatedEvent(Guid ProductId) : IEvent;
