using ERP.Products.Persistance.PgSQL.BaseRepository.Read;
using ERP.Products.Persistance.PgSQL.BaseRepository.Write;
using ERP.Products.Persistance.PgSQL.Data.Read;
using ERP.Products.Persistance.PgSQL.Data.Write;
using ERP.Products.Persistance.PgSQL.Options;
using ERP.Products.Persistance.PgSQL.Storages.Outbox;
using ERP.Shared.Application.Abstractions.Interfaces;
using ERP.Shared.Application.Abstractions.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Persistance.PgSQL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPgInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProductDbContextServices(configuration);

        return services;
    }

    private static IServiceCollection AddProductDbContextServices(this IServiceCollection services, IConfiguration configuration)
    {
        //A bounded context may have 2 dbcontexts (read & write CQRS pattern)
        //A dbcontext may has multiple aggregate roots.

        //Write DbContext. 
        services.AddDbContextPool<ProductDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            PgOptions? pgOption = configuration.GetSection("PgDbSettings").Get<PgOptions>();

            if (string.IsNullOrWhiteSpace(pgOption?.WriteConnectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{pgOption?.WriteConnectionString}' was not found. " +
                    $"Store it in configuration as ConnectionStrings:{pgOption?.WriteConnectionString}.");
            }

            options.UseNpgsql(pgOption?.WriteConnectionString);
        });

        //Read DbContext. 
        services.AddDbContextPool<ProductReadDbContext>((serviceProvider, options) =>
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

            PgOptions? pgOption = configuration.GetSection("PgDbSettings").Get<PgOptions>();

            if (string.IsNullOrWhiteSpace(pgOption?.ReadConnectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{pgOption?.ReadConnectionString}' was not found. " +
                    $"Store it in configuration as ConnectionStrings:{pgOption?.ReadConnectionString}.");
            }

            options.UseNpgsql(pgOption?.ReadConnectionString);
        });

        //A read dbcontext should have 1 generic read repository for all aggregate roots in a bounded context.
        //A write dbcontext should have 1 generic write repository for all aggregate roots in a bounded context.
        services.AddScoped(typeof(IReadRepository<,>), typeof(PgReadRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(PgRepository<,>));

        //A write dbcontext should have 1 outbox storage for integration events.
        services.AddScoped<IOutboxStorage, OutboxStorage>();

        //A write dbcontext should have 1 unit of work for all aggregate roots in a bounded context.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //A bounded context pick a database and that should have 1 query executor for all async queries (multiple dbcontexts).
        services.AddScoped<IQueryExecutor, QueryExecutor>();

        return services;
    }
}






