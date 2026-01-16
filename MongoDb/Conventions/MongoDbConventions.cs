using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using System.Threading;

namespace MongoDb.Conventions;

public static class MongoDbConventions
{
    private static int _registered;

    public static void Register()
    {
        if (Interlocked.Exchange(ref _registered, 1) == 1)
            return;

        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new EnumRepresentationConvention(BsonType.String),
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfNullConvention(true)
        };

        ConventionRegistry.Register("MongoDbConventions", conventionPack, _ => true);

        BsonSerializer.RegisterSerializer(DateTimeSerializer.UtcInstance);
        BsonSerializer.RegisterSerializer(typeof(DateTime?), new NullableSerializer<DateTime>(DateTimeSerializer.UtcInstance));
    }
}
