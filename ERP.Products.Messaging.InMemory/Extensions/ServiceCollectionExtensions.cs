using ERP.Products.Messaging.InMemory.Jobs;
using ERP.Shared.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Messaging.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IOutboxJob, OutboxJob>();

        return services;
    }
}



