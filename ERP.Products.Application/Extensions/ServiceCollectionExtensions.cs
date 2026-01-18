using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
namespace ERP.Products.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}



