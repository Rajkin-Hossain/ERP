using ERP.Shared.Domain.Interfaces;
using ERP.Shared.Event.Contracts.Interfaces;

namespace ERP.Products.Application.Abstraction.Interfaces;

public interface IIntegrationEventMapper
{
    IReadOnlyCollection<IEvent> Map(IReadOnlyCollection<IDomainEvent> domainEvents);
}
