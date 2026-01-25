using ERP.Products.Persistance.MongoDb.Data;
using ERP.Products.Persistance.MongoDb.Options;
using ERP.Products.Persistance.MongoDb.QueryExecutor;
using ERP.Products.Persistance.MongoDb.Repositories;
using ERP.Products.Persistance.MongoDb.UnitOfWorks;
using ERP.Shared.Application.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
namespace ERP.Products.Persistance.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(MongoOptions.SectionName);
        services.Configure<MongoOptions>(section);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(section.Get<MongoOptions>()?.ConnectionString));

        services.AddDbContext<ProductDbContext>((provider, options) =>
        {
            var mongoOptions = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoOptions>>().Value;
            var client = provider.GetRequiredService<IMongoClient>();
            options.UseMongoDB(client, mongoOptions.WriteDatabaseName);
        });

        services.AddDbContext<ProductReadDbContext>((provider, options) =>
        {
            var mongoOptions = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoOptions>>().Value;
            var client = provider.GetRequiredService<IMongoClient>();
            options.UseMongoDB(client, mongoOptions.WriteDatabaseName);
        });

        services.AddScoped(typeof(IReadRepository<,>), typeof(ProductReadRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(ProductRepository<,>));

        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUnitOfWork, ProductContextUnitOfWork>();

        services.AddScoped<IAsyncQueryExecutor, MongoAsyncQueryExecutor>();

        return services;
    }
}






