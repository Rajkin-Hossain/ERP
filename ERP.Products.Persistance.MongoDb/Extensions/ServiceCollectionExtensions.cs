using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Persistence.MongoDb.Extensions;
using BuildingBlocks.Persistence.MongoDb.QueryExecutor;
using ERP.Products.Application.Interfaces;
using ERP.Products.Persistance.MongoDb.Data;
using ERP.Products.Persistance.MongoDb.Repositories;
using ERP.Products.Persistance.MongoDb.UnitOfWorks;
using ERP.SharedKernal.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Persistance.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAsyncQueryExecutor, MongoAsyncQueryExecutor>();

        services.AddMongoServices<ProductDbContext>(configuration);

        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

        services.AddScoped<IUnitOfWork, ProductContextUnitOfWork>();

        return services;
    }
}
