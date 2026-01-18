using ERP.MessageOrchestrator.Contracts.Interfaces;
using ERP.MessageOrchestrator.Contracts.Products.MessageEvents;
using ERP.Products.Domain.DomainEvents;
using System.Text.Json;

namespace ERP.Products.Messaging.RabbitMQ;

public static class IntegrationEventMapper
{
    private static readonly Dictionary<string, Func<string, IMessageEvent>> _map = new()
    {
        [typeof(ProductCreatedDomainEvent).FullName!] = payload =>
        {
            var e = JsonSerializer.Deserialize<ProductCreatedDomainEvent>(payload)
                     ?? throw new InvalidOperationException("Bad payload");

            return new ProductCreatedMessageEvent(
                Guid.NewGuid()
            );
        },

        [typeof(ProductUpdatedDomainEvent).FullName!] = payload =>
        {
            var e = JsonSerializer.Deserialize<ProductUpdatedDomainEvent>(payload)
                     ?? throw new InvalidOperationException("Bad payload");

            return new ProductUpdatedMessageEvent(
                Guid.NewGuid()
            );
        }
    };

    public static IMessageEvent Map(string eventType, string payload)
    {
        if (!_map.TryGetValue(eventType, out var factory))
            throw new NotSupportedException($"No mapping found for {eventType}");

        return factory(payload);
    }
}





