using EFCore.QueryExecutor;
using ERP.SharedKernal.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryExecutor, EfCoreAsyncQueryExecutor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
