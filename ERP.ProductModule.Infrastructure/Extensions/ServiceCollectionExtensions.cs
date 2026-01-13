using EFCore.Extensions;
using ERP.ProductModule.Application.RepoInterfaces;
using ERP.ProductModule.Infrastructure.Data.ProductContext;
using ERP.ProductModule.Infrastructure.Repositories.ProductContext;
using ERP.ProductModule.Infrastructure.UnitOfWorks.ProductContext;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.ProductModule.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseConnectionStringName = "erp";

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Add DI for Library
        services.AddEfCoreServices();

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

        services.AddScoped<IUnitOfWork, ProductContextUnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
