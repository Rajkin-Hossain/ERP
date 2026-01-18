using BuildingBlocks.Messaging.RabbitMq.Extensions;
using ERP.Products.Messaging.RabbitMQ.Outbox;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ERP.Products.Messaging.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddMessageBusInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ProductOutboxJob>();

        services.AddMessageBus(Assembly.GetExecutingAssembly()); //consumer assembly

        return services;
    }
}
