using BuildingBlocks.Messaging.RabbitMq.Extensions;
using ERP.Products.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ERP.Products.Messaging.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddMessageBusInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductOutboxDispatcher, ProductOutboxDispatcher>();

        services.AddMessageBus(Assembly.GetExecutingAssembly()); //consumer assembly

        return services;
    }
}
