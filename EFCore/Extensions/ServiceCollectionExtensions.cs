using EFCore.QueryExecutor;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfCoreServices<T>(this IServiceCollection services, 
        string writeDB, 
        string readDB) 
        where T : DbContext
    {
        services.AddScoped<IAsyncQueryExecutor, EfCoreAsyncQueryExecutor>();

        services.AddDbContextPool<T>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connectionString = configuration.GetConnectionString(writeDB);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{writeDB}' was not found. " +
                    $"Store it in configuration as ConnectionStrings:{writeDB}.");
            }

            options.UseNpgsql(connectionString);
        });

        /*
        services.AddDbContextPool<ProductDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connectionString = configuration.GetConnectionString(DatabaseReadConnectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{DatabaseReadConnectionStringName}' was not found. " +
                    $"Store it in configuration as ConnectionStrings:{DatabaseReadConnectionStringName}.");
            }

            options.UseNpgsql(connectionString);
        });
        */

        return services;
    }
}
