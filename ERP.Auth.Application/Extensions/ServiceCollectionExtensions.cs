using ERP.Auth.Application.UserCases;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Auth.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services)
    {
        services.AddScoped<AuthService>();

        return services;
    }
}
