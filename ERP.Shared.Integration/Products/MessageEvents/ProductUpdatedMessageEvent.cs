using ERP.Shared.Integration.Interfaces;

namespace ERP.Shared.Integration.Products.MessageEvents;

public record ProductUpdatedMessageEvent(Guid ProductId) : IMessageEvent;




