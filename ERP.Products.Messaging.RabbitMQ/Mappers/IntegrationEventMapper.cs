using ERP.Contracts.MessageOrchestrator.Interfaces;
using ERP.Contracts.MessageOrchestrator.Products.MessageEvents;
using ERP.Products.Domain.DomainEvents;
using System.Text.Json;

namespace ERP.Products.Messaging.RabbitMQ.Mappers;

public static class IntegrationEventMapper
{
    private static readonly Dictionary<string, Func<JsonElement, IMessageEvent>> _map = new()
    {
        [typeof(ProductCreatedDomainEvent).FullName!] = root => new ProductCreatedMessageEvent(
            GetGuid(root, nameof(ProductCreatedDomainEvent.ProductId)),
            GetString(root, nameof(ProductCreatedDomainEvent.ProductName)),
            GetGuid(root, nameof(ProductCreatedDomainEvent.CategoryId)),
            GetString(root, nameof(ProductCreatedDomainEvent.ImageUrl)),
            GetDecimal(root, nameof(ProductCreatedDomainEvent.Price))
        ),

        [typeof(ProductUpdatedDomainEvent).FullName!] = root => new ProductUpdatedMessageEvent(
            GetGuid(root, nameof(ProductUpdatedDomainEvent.ProductId)),
            GetString(root, nameof(ProductUpdatedDomainEvent.ProductName)),
            GetGuid(root, nameof(ProductUpdatedDomainEvent.CategoryId)),
            GetString(root, nameof(ProductUpdatedDomainEvent.ImageUrl)),
            GetDecimal(root, nameof(ProductUpdatedDomainEvent.Price))
        ),

        [typeof(ProductPriceUpdatedDomainEvent).FullName!] = root => new ProductPriceUpdatedMessageEvent(
            GetGuid(root, nameof(ProductPriceUpdatedDomainEvent.ProductId)),
            GetDecimal(root, nameof(ProductPriceUpdatedDomainEvent.Price))
        )
    };

    public static IMessageEvent Map(string eventType, string payload)
    {
        if (!_map.TryGetValue(eventType, out var factory))
            throw new NotSupportedException($"No mapping found for {eventType}");

        using var doc = JsonDocument.Parse(payload);
        return factory(doc.RootElement);
    }

    private static JsonElement GetValueElement(JsonElement root, string propertyName)
    {
        // 1. Try exact match (fastest)
        if (root.TryGetProperty(propertyName, out var prop))
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
        foreach (var property in root.EnumerateObject())
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
            if (prop.TryGetProperty("Value", out var innerValue)) return innerValue;
            if (prop.TryGetProperty("value", out var innerValueLower)) return innerValueLower;
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
