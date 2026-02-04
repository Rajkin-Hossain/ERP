using ERP.Products.QueryApplication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.QueryApplication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<ProductService>();

        return services;
    }
}




