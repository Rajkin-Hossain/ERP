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
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddProductDbContextServices();

        return services;
    }

    private static IServiceCollection AddProductDbContextServices(this IServiceCollection services)
    {
        services.AddDbContextPool<ProductDbContext>((serviceProvider, options) =>
        {
            var pgOptions = GetPgOptions(serviceProvider);
            ConfigureDbContext(options, pgOptions.WriteConnectionString, nameof(PgOptions.WriteConnectionString));
        });

        services.AddDbContextPool<ProductReadDbContext>((serviceProvider, options) =>
        {
            var pgOptions = GetPgOptions(serviceProvider);
            ConfigureDbContext(options, pgOptions.WriteConnectionString, nameof(PgOptions.WriteConnectionString));
        });

        services.AddScoped(typeof(IReadRepository<,>), typeof(PgReadRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(PgRepository<,>));

        services.AddScoped<IOutboxStorage, OutboxStorage>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IQueryExecutor, QueryExecutor>();

        return services;
    }

    private static PgOptions GetPgOptions(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var options = configuration.GetSection(PgOptions.SectionName).Get<PgOptions>();

        return options ?? throw new InvalidOperationException(
            $"Configuration section '{PgOptions.SectionName}' was not found.");
    }

    private static void ConfigureDbContext(
        DbContextOptionsBuilder optionsBuilder,
        string connectionString,
        string connectionName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{connectionName}' was not found in '{PgOptions.SectionName}'.");
        }

        optionsBuilder.UseNpgsql(connectionString);
    }
}






