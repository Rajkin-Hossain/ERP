using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.ProductModule.Application.RepoInterfaces.Write;
using ERP.ProductModule.MongoDb.Data;
using ERP.ProductModule.MongoDb.Indexes;
using ERP.ProductModule.MongoDb.Outbox;
using ERP.ProductModule.MongoDb.QueryExecutor;
using ERP.ProductModule.MongoDb.Repositories;
using ERP.ProductModule.MongoDb.Repositories.Read;
using ERP.ProductModule.MongoDb.Repositories.Write;
using ERP.ProductModule.MongoDb.UnitOfWorks;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDb.Options;
using MongoDB.EntityFrameworkCore.Extensions;
using MongoDB.Driver;

namespace ERP.ProductModule.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IAsyncQueryExecutor, EfCoreAsyncQueryExecutor>();

        services.AddScoped<ProductOutboxJob>();

        var section = configuration.GetSection(MongoOptions.SectionName);
        services.Configure<MongoOptions>(section);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(section.Get<MongoOptions>()?.ConnectionString));

        services.AddDbContext<ProductDbContext>((provider, options) =>
        {
            var mongoOptions = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoOptions>>().Value;
            var client = provider.GetRequiredService<IMongoClient>();
            options.UseMongoDB(client, mongoOptions.WriteDatabaseName);
        });

        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

        services.AddScoped<IProductOutboxRepository, ProductOutboxRepository>();

        services.AddScoped<IUnitOfWork, ProductContextUnitOfWork>();
        services.AddHostedService<ProductIndexInitializer>();

        return services;
    }
}
