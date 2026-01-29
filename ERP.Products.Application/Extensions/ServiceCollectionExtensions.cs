using ERP.Products.Application.Abstraction.Interfaces;
using ERP.Products.Application.Integration;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IIntegrationEventMapper, ProductIntegrationEventMapper>();

        return services;
    }
}
