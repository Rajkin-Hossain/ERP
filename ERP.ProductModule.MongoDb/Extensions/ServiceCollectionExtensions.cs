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
using MongoDb.QueryExecutor;

namespace ERP.ProductModule.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IAsyncQueryExecutor, MongoAsyncQueryExecutor>();

        services.AddScoped<ProductOutboxJob>();

        services.AddMongoServices<ProductDbContext>(configuration);

        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

        services.AddScoped<IProductOutboxRepository, ProductOutboxRepository>();

        services.AddScoped<IUnitOfWork, ProductContextUnitOfWork>();

        return services;
    }
}
