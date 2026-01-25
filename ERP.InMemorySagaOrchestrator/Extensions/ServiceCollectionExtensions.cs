using Microsoft.Extensions.DependencyInjection;

namespace ERP.InMemorySagaOrchestrator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrchestratorServices
        (this IServiceCollection services)
    {
        services.AddSingleton<Saga>();

        return services;
    }
}

