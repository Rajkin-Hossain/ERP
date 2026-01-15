using Microsoft.Extensions.Options;
using MongoDb.DbContext;
using MongoDb.Options;
using MongoDB.Driver;

namespace ERP.ProductModule.MongoDb.Data;

public class ProductWriteDbContext : MongoDbContext
{
    public ProductWriteDbContext(IMongoClient client, IOptions<MongoOptions> options)
        : base(client, options.Value.WriteDatabaseName)
    {
    }
}
