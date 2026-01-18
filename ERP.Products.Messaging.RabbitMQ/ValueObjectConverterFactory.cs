using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Products.Messaging.RabbitMQ;

public class ValueObjectConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Check if it has a property named "Value"
        var valueProperty = typeToConvert.GetProperty("Value");
        if (valueProperty == null) return false;

        // Check if it has a constructor (public or private) that takes the value type
        var constructor = typeToConvert.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            [valueProperty.PropertyType],
            null);

        return constructor != null;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueProperty = typeToConvert.GetProperty("Value")!;
        var converterType = typeof(ValueObjectConverter<,>).MakeGenericType(typeToConvert, valueProperty.PropertyType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private class ValueObjectConverter<TValueObject, TValue> : JsonConverter<TValueObject>
        where TValueObject : class
    {
        public override TValueObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return default;

            TValue? value = default;

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                // Handle nested {"Value": "..."}
                using var jsonDoc = JsonDocument.ParseValue(ref reader);
                if (jsonDoc.RootElement.TryGetProperty("Value", out var propertyElement))
                {
                    value = propertyElement.Deserialize<TValue>(options);
                }
                else if (jsonDoc.RootElement.TryGetProperty("value", out var propertyElementLower))
                {
                    value = propertyElementLower.Deserialize<TValue>(options);
                }
            }
            else
            {
                // Handle flat value "..." or primitive
                value = JsonSerializer.Deserialize<TValue>(ref reader, options);
            }

            if (value is null) return default;

            // Find the constructor (public or private) that takes TValue
            var constructor = typeof(TValueObject).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                [typeof(TValue)],
                null);

            if (constructor != null)
            {
                return (TValueObject)constructor.Invoke([value]);
            }

            throw new InvalidOperationException($"No suitable constructor found for {typeof(TValueObject).Name}");
        }

        public override void Write(Utf8JsonWriter writer, TValueObject value, JsonSerializerOptions options)
        {
            var property = typeof(TValueObject).GetProperty("Value");
            var underlyingValue = property?.GetValue(value);
            JsonSerializer.Serialize(writer, underlyingValue, options);
        }
    }
}
