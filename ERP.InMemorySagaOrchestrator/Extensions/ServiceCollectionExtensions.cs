using Microsoft.Extensions.DependencyInjection;

namespace ERP.InMemorySagaOrchestrator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSagaOrchestratorServices
        (this IServiceCollection services)
    {
        services.AddScoped<Saga>();

        return services;
    }
}

