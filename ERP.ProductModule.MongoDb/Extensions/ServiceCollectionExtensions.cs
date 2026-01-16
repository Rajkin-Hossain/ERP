using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.ProductModule.Application.RepoInterfaces.Write;
using ERP.ProductModule.MongoDb.Data;
using ERP.ProductModule.MongoDb.Outbox;
using ERP.ProductModule.MongoDb.Repositories;
using ERP.ProductModule.MongoDb.Repositories.Read;
using ERP.ProductModule.MongoDb.Repositories.Write;
using ERP.ProductModule.MongoDb.UnitOfWorks;
using ERP.SharedKernal.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDb.Extensions;
using MongoDb.Options;
using MongoDb.QueryExecutor;
using MongoDB.Driver;
using System.Reflection;

namespace ERP.ProductModule.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IAsyncQueryExecutor, MongoAsyncQueryExecutor>();

        services.RegisterMongoConfigurations(Assembly.GetExecutingAssembly());

        services.AddScoped<ProductOutboxJob>();

        var section = configuration.GetSection("MongoDbSettings");
        services.Configure<MongoOptions>(section);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(section.Get<MongoOptions>()?.ConnectionString));

        services.AddSingleton<ProductReadDbContext>();
        services.AddSingleton<ProductWriteDbContext>();

        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

        services.AddScoped<IProductOutboxRepository, ProductOutboxRepository>();

        services.AddScoped<IUnitOfWork, ProductContextUnitOfWork>();

        return services;
    }
}
