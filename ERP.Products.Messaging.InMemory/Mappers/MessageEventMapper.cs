using ERP.Products.Domain.DomainEvents;
using ERP.Shared.Event.Contracts.Interfaces;
using ERP.Shared.Event.Contracts.Modules.Products.Events;
using System.Text.Json;

namespace ERP.Products.Messaging.InMemory.Mappers;

public static class MessageEventMapper
{
    private static readonly Dictionary<string, Func<JsonElement, IEvent>> _map = new()
    {
        [typeof(ProductCreatedDomainEvent).FullName!] = root => new ProductCreatedEvent(
            GetGuid(root, nameof(ProductCreatedDomainEvent.ProductId)),
            GetString(root, nameof(ProductCreatedDomainEvent.ProductName)),
            GetGuid(root, nameof(ProductCreatedDomainEvent.CategoryId)),
            GetString(root, nameof(ProductCreatedDomainEvent.ImageUrl)),
            GetDecimal(root, nameof(ProductCreatedDomainEvent.Price))
        )
    };

    public static IEvent Map(string eventType, string payload)
    {
        if (!_map.TryGetValue(eventType, out Func<JsonElement, IEvent>? factory))
            throw new NotSupportedException($"No mapping found for {eventType}");

        using JsonDocument doc = JsonDocument.Parse(payload);
        return factory(doc.RootElement);
    }

    private static JsonElement GetValueElement(JsonElement root, string propertyName)
    {
        // 1. Try exact match (fastest)
        if (root.TryGetProperty(propertyName, out JsonElement prop))
        {
            return ExtractValue(prop);
        }

        // 2. Try camelCase (common convention)
        var camelCase = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        if (root.TryGetProperty(camelCase, out prop))
        {
            return ExtractValue(prop);
        }

        // 3. Fallback to full case-insensitive search (slowest but safest)
        foreach (JsonProperty property in root.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                return ExtractValue(property.Value);
            }
        }

        throw new InvalidOperationException($"Property '{propertyName}' not found in payload.");
    }

    private static JsonElement ExtractValue(JsonElement prop)
    {
        // Handle Value Object wrapper { "Value": ... } or { "value": ... }
        if (prop.ValueKind == JsonValueKind.Object)
        {
            if (prop.TryGetProperty("Value", out JsonElement innerValue)) return innerValue;
            if (prop.TryGetProperty("value", out JsonElement innerValueLower)) return innerValueLower;
        }

        return prop;
    }

    private static Guid GetGuid(JsonElement root, string propertyName)
        => GetValueElement(root, propertyName).GetGuid();

    private static string GetString(JsonElement root, string propertyName)
        => GetValueElement(root, propertyName).GetString() ?? string.Empty;

    private static decimal GetDecimal(JsonElement root, string propertyName)
        => GetValueElement(root, propertyName).GetDecimal();
}
