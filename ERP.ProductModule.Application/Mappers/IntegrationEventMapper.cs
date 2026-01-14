using ERP.Orchestrator.Contract.Interfaces;
using ERP.Orchestrator.Contract.ProductModule.Events;
using ERP.ProductModule.Domain.DomainEvents;
using System.Text.Json;

namespace ERP.ProductModule.Application.Mappers;

public static class IntegrationEventMapper
{
    private static readonly Dictionary<string, Func<string, IEvent>> _map = new()
    {
        [nameof(ProductCreatedDomainEvent)] = payload =>
        {
            var e = JsonSerializer.Deserialize<ProductCreatedDomainEvent>(payload)
                     ?? throw new InvalidOperationException("Bad payload");

            return new ProductCreatedEvent(
                Guid.NewGuid()
            );
        },

        [nameof(ProductUpdatedDomainEvent)] = payload =>
        {
            var e = JsonSerializer.Deserialize<ProductUpdatedDomainEvent>(payload)
                     ?? throw new InvalidOperationException("Bad payload");

            return new ProductUpdatedEvent(
                Guid.NewGuid()
            );
        }
    };

    public static IEvent Map(string eventType, string payload)
    {
        if (!_map.TryGetValue(eventType, out var factory))
            throw new NotSupportedException($"No mapping found for {eventType}");

        return factory(payload);
    }
}
