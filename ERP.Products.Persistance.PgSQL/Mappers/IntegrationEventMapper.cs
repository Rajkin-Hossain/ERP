using ERP.Products.Domain.DomainEvents;
using ERP.Shared.Domain.Interfaces;
using ERP.Shared.Event.Contracts.Interfaces;
using ERP.Shared.Event.Contracts.Modules.Products;

namespace ERP.Products.Persistance.PgSQL.Mappers;

public sealed class IntegrationEventMapper
{
    public static IReadOnlyCollection<IEvent> Map(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        if (domainEvents is null || domainEvents.Count == 0)
        {
            return [];
        }

        var integrationEvents = new List<IEvent>(domainEvents.Count);

        foreach (var domainEvent in domainEvents)
        {
            if (TryMap(domainEvent, out var integrationEvent))
            {
                integrationEvents.Add(integrationEvent!);
            }
        }

        return integrationEvents;
    }

    private static bool TryMap(IDomainEvent domainEvent, out IEvent? integrationEvent)
    {
        switch (domainEvent)
        {
            case ProductCreatedDomainEvent created:
                integrationEvent = new ProductCreatedEvent(
                    created.ProductId.Value,
                    created.ProductName.Value,
                    created.CategoryId.Value,
                    created.ImageUrl.Value,
                    created.Price.Value);
                return true;
            default:
                integrationEvent = null;
                return false;
        }
    }
}
