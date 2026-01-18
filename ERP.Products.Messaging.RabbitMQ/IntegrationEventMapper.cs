using ERP.MessageOrchestrator.Contracts.Interfaces;
using ERP.MessageOrchestrator.Contracts.Products.MessageEvents;
using ERP.Products.Domain.DomainEvents;
using System.Text.Json;

namespace ERP.Products.Messaging.RabbitMQ;

public static class IntegrationEventMapper
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new ValueObjectConverterFactory() },
        PropertyNameCaseInsensitive = true
    };

    private static readonly Dictionary<string, Func<string, IMessageEvent>> _map = new()
    {
        [typeof(ProductCreatedDomainEvent).FullName!] = payload =>
        {
            var e = JsonSerializer.Deserialize<ProductCreatedDomainEvent>(payload, _jsonOptions)
                     ?? throw new InvalidOperationException("Bad payload");

            return new ProductCreatedMessageEvent(
                e.ProductId.Value,
                e.ProductName.Value,
                e.CategoryId.Value,
                e.ImageUrl.Value,
                e.Price.Value
            );
        },

        [typeof(ProductUpdatedDomainEvent).FullName!] = payload =>
        {
            var e = JsonSerializer.Deserialize<ProductUpdatedDomainEvent>(payload, _jsonOptions)
                     ?? throw new InvalidOperationException("Bad payload");

            return new ProductUpdatedMessageEvent(
                e.ProductId.Value,
                e.ProductName.Value,
                e.CategoryId.Value,
                e.ImageUrl.Value,
                e.Price.Value
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
