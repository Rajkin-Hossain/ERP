using MongoDB.Driver;

namespace MongoDb.DbContext;

public abstract class MongoDbContext
{
    protected readonly IMongoDatabase _database;

    protected MongoDbContext(IMongoClient client, string databaseName)
    {
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> Set<T>()
    {
        return _database.GetCollection<T>(typeof(T).Name);
    }
}
