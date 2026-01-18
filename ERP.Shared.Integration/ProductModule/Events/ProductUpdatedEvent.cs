using ERP.Shared.Integration.Interfaces;

namespace ERP.Shared.Integration.ProductModule.Events;

public record ProductUpdatedEvent(Guid ProductId) : IEvent;



