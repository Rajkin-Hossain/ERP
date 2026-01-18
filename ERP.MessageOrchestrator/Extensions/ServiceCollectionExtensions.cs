using ERP.Products.Messaging.RabbitMQ.Extensions;
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
}

