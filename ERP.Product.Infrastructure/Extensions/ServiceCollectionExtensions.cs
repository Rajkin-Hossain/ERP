using ERP.Product.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Product.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseConnectionStringName = "erp";

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContextPool<ProductDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connectionString = configuration.GetConnectionString(DatabaseConnectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{DatabaseConnectionStringName}' was not found. " +
                    $"Store it in configuration as ConnectionStrings:{DatabaseConnectionStringName}.");
            }

            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
