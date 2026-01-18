using BuildingBlocks.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.RabbitMq.Extensions;

public static class ServiceCollectionExtensions
{
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
