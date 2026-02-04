using ERP.Products.CommandApplication.Extensions;
using ERP.Products.QueryApplication.Extensions;

namespace ERP.Products.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddCommandApplicationServices();
        services.AddQueryApplicationServices();

        return services;
    }
}




