using ERP.MessageOrchestrator.Interfaces;
using MassTransit;
using System.Reflection;

namespace ERP.MessageOrchestrator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrchestratorServices
        (this IServiceCollection services)
    {
        services.AddMessageBus(Assembly.GetExecutingAssembly());

        services.AddScoped<ProductManagementOrchestrator>();

        return services;
    }

    public static IServiceCollection AddMessageBusInfrastructure(this IServiceCollection services)
    {
        return services.AddMessageBus(typeof(ServiceCollectionExtensions).Assembly);
    }

    public static IServiceCollection AddMessageBus(
        this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IServiceBus, RabbitMQBus>();

        services.AddMassTransit(config =>
        {
            // Consumer discovery
            config.AddConsumers(assemblies);

            // RabbitMQ configuration
            config.UsingRabbitMq((context, cfg) =>
            {
                var configuration = context.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("rabbitmq");

                cfg.Host(connectionString);
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

