using EFCore.Extensions;
using ERP.ProductModule.Application.RepoInterfaces;
using ERP.ProductModule.Infrastructure.Data.ProductContext;
using ERP.ProductModule.Infrastructure.RabbitMQ;
using ERP.ProductModule.Infrastructure.Repositories.ProductContext;
using ERP.ProductModule.Infrastructure.UnitOfWorks.ProductContext;
using ERP.SharedKernal.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Extensions;
using System.Reflection;

namespace ERP.ProductModule.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseWriteConnectionStringName = "erp_write";
    private const string DatabaseReadConnectionStringName = "erp_read";

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Add DI for Library
        services.AddEfCoreServices();

        services.AddDbContextPool<ProductDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connectionString = configuration.GetConnectionString(DatabaseWriteConnectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{DatabaseWriteConnectionStringName}' was not found. " +
                    $"Store it in configuration as ConnectionStrings:{DatabaseWriteConnectionStringName}.");
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

        services.AddMessageBus(Assembly.GetExecutingAssembly());

        services.AddScoped<IUnitOfWork, ProductContextUnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
