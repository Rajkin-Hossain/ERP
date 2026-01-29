using ERP.Shared.Event.Contracts.Interfaces;
using ERP.Shared.Event.Contracts.Modules.Products;
using System.Text.Json;

namespace ERP.Products.Messaging.InMemory.Mappers;

public static class MessageEventMapper
{
    private static readonly Dictionary<string, Func<string, IEvent>> _map = new()
    {
        [typeof(ProductCreatedEvent).FullName!] = payload => Deserialize<ProductCreatedEvent>(payload)
    };

    public static IEvent Map(string eventType, string payload)
    {
        if (string.IsNullOrWhiteSpace(eventType))
            throw new ArgumentException("Event type is required.", nameof(eventType));

        if (!_map.TryGetValue(eventType, out Func<string, IEvent>? factory))
            throw new NotSupportedException($"No mapping found for {eventType}");

        return factory(payload);
    }

    private static T Deserialize<T>(string payload) where T : class, IEvent
    {
        if (string.IsNullOrWhiteSpace(payload))
            throw new ArgumentException("Payload is required.", nameof(payload));

        var message = JsonSerializer.Deserialize<T>(payload);
        return message ?? throw new InvalidOperationException("Failed to deserialize integration event payload.");
    }
}
