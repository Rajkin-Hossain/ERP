using ERP.ProductModule.MongoDb.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDb.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ERP.ProductModule.MongoDb.Indexes;

public sealed class ProductIndexInitializer(
    IMongoClient client,
    IOptions<MongoOptions> options) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var database = client.GetDatabase(options.Value.WriteDatabaseName);

        await EnsureProductIndexesAsync(database, cancellationToken);
        await EnsureCategoryIndexesAsync(database, cancellationToken);
        await EnsureOutboxIndexesAsync(database, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static Task EnsureProductIndexesAsync(IMongoDatabase database, CancellationToken ct)
    {
        var collection = database.GetCollection<BsonDocument>(ProductConfiguration.CollectionName);
        var indexModels = new List<CreateIndexModel<BsonDocument>>
        {
            new(Builders<BsonDocument>.IndexKeys.Ascending("CategoryId")),
            new(Builders<BsonDocument>.IndexKeys.Ascending("ProductName"))
        };

        return collection.Indexes.CreateManyAsync(indexModels, ct);
    }

    private static Task EnsureCategoryIndexesAsync(IMongoDatabase database, CancellationToken ct)
    {
        var collection = database.GetCollection<BsonDocument>(CategoryConfiguration.CollectionName);
        var indexModels = new List<CreateIndexModel<BsonDocument>>
        {
            new(Builders<BsonDocument>.IndexKeys.Ascending("CategoryName"))
        };

        return collection.Indexes.CreateManyAsync(indexModels, ct);
    }

    private static Task EnsureOutboxIndexesAsync(IMongoDatabase database, CancellationToken ct)
    {
        var collection = database.GetCollection<BsonDocument>(ProductOutboxMessageConfiguration.CollectionName);
        var indexModels = new List<CreateIndexModel<BsonDocument>>
        {
            new(Builders<BsonDocument>.IndexKeys.Ascending("Status").Ascending("OccurredOnUtc"))
        };

        return collection.Indexes.CreateManyAsync(indexModels, ct);
    }
}
