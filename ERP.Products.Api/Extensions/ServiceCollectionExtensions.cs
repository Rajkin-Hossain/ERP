using ERP.Products.CommandApplication.Extensions;
using ERP.Products.QueryApplication.Extensions;

namespace ERP.Products.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCommandApplicationServices();
        services.AddQueryApplicationServices();

        return services;
    }
}




