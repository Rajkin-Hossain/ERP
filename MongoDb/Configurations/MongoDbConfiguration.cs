using MongoDB.Bson.Serialization;

namespace MongoDb.Configurations;

public abstract class MongoDbConfiguration<T> : IMongoDbConfiguration
{
    public virtual void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        {
            BsonClassMap.RegisterClassMap<T>(map =>
            {
                map.AutoMap();
                ApplyConfiguration(map);
            });
        }
    }

    protected abstract void ApplyConfiguration(BsonClassMap<T> map);
}
