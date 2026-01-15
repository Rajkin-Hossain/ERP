using ERP.SharedKernal.Interfaces;
using RabbitMQ;
using RabbitMQ.Extensions;
using System.Reflection;

namespace ERP.Orchestrator.Extensions;

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
