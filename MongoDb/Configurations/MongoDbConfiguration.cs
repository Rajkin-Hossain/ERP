using MongoDB.Bson.Serialization;

namespace MongoDb.Configurations;

public abstract class MongoDbConfiguration<T> : IMongoConfiguration
{
    public virtual void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        {
            BsonClassMap.RegisterClassMap<T>(Configure);
        }
    }

    protected abstract void Configure(BsonClassMap<T> map);
}
