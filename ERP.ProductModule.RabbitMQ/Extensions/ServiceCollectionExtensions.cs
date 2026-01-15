using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Extensions;
using System.Reflection;

namespace ERP.ProductModule.RabbitMQ.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddMessageBusInfrastructure(this IServiceCollection services)
    {
        services.AddMessageBus(Assembly.GetExecutingAssembly()); //consumer assembly

        return services;
    }
}
