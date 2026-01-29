using ERP.Products.Application.Extensions;

namespace ERP.Products.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddProductApplication();

        return services;
    }
}




