using ERP.Products.Domain.Entities;
using ERP.Products.Application.Interfaces;
namespace ERP.Products.Persistance.MongoDb.Options;

public class MongoOptions
{
    public const string SectionName = "MongoDbSettings";

    public string ConnectionString { get; set; } = string.Empty;
    public string WriteDatabaseName { get; set; } = string.Empty;
    public string ReadDatabaseName { get; set; } = string.Empty;
}




