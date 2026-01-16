using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDb.Serializers;

public class SingleValueObjectSerializer<TObject, TValue> : SerializerBase<TObject>, IBsonDocumentSerializer
    where TObject : class
{
    private readonly Func<TValue, TObject> _factory;
    private readonly Func<TObject, TValue> _valueAccessor;

    public SingleValueObjectSerializer(Func<TValue, TObject> factory, Func<TObject, TValue> valueAccessor)
    {
        _factory = factory;
        _valueAccessor = valueAccessor;
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TObject value)
    {
        if (value == null)
        {
            context.Writer.WriteNull();
            return;
        }

        var innerValue = _valueAccessor(value);
        BsonSerializer.Serialize(context.Writer, innerValue);
    }

    public override TObject Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull();
            return null;
        }

        try
        {
            var innerValue = BsonSerializer.Deserialize<TValue>(context.Reader);
            return _factory(innerValue);
        }
        catch (Exception ex)
        {
             throw new BsonSerializationException($"Error deserializing {typeof(TObject).Name}", ex);
        }
    }

    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
    {
        // This is the critical implementation for LINQ translation
        // When LINQ tries to access 'p.ProductName.Value', it looks here.
        // We forward the request to the inner serializer (e.g., StringSerializer for string).
        
        if (memberName == "Value") // Assuming your Value Objects all use "Value" as the property name
        {
            var innerSerializer = BsonSerializer.LookupSerializer<TValue>();
            serializationInfo = new BsonSerializationInfo(
                elementName: null, // Because it's the value itself, it doesn't have a sub-element name in the DB
                serializer: innerSerializer,
                nominalType: typeof(TValue)
            );
            return true;
        }

        serializationInfo = null;
        return false;
    }
}
