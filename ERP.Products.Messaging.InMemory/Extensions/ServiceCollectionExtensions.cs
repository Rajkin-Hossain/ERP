using ERP.Products.Application.Abstraction.Interfaces;
using ERP.Products.Messaging.InMemory.Dispatcher;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Messaging.InMemory.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductOutboxDispatcher, ProductOutboxDispatcher>();

        return services;
    }
}



