using ERP.Shared.Integration.Interfaces;

namespace ERP.Shared.Integration.ProductModule.Events;

public record ProductCreatedEvent(Guid ProductId) : IEvent;



