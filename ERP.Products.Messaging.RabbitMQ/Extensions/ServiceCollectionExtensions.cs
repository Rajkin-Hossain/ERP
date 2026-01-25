using ERP.Products.CommandHandlers.Interfaces;
using ERP.Products.Messaging.RabbitMQ.Bus;
using ERP.Products.Messaging.RabbitMQ.Dispatcher;
using ERP.Products.Messaging.RabbitMQ.Interfaces;
using ERP.Shared.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace ERP.Products.Messaging.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBusInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, ProductOutboxDispatcher>();
        services.AddScoped<IServiceBus, RabbitMQBus>();

        return services.AddMessageBus(typeof(ServiceCollectionExtensions).Assembly);
    }

    public static IServiceCollection AddMessageBus(
        this IServiceCollection services, params Assembly[] assemblies)
    {
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



