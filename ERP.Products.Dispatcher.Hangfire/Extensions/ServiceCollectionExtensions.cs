using ERP.Products.Application.Interfaces;
using Hangfire;
using Hangfire.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Dispatcher.Hangfire.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHangfireInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IOutboxDispatchTrigger, OutboxDispatchTrigger>();

        services.AddHangfire(cfg =>
        {
            cfg.UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               // Use the In-Memory storage instead of Redis
               .UseInMemoryStorage(new InMemoryStorageOptions
               {
                   // Limits memory growth on your 8GB RAM PC
                   MaxExpirationTime = TimeSpan.FromHours(1),
                   IdType = InMemoryStorageIdType.Long
               });
        });

        services.AddHangfireServer();

        return services;
    }
}




