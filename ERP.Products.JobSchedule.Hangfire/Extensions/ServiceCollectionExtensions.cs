using ERP.Products.JobSchedule.Hangfire.JobSchedulers;
using ERP.Shared.Application.Abstractions.Interfaces;
using Hangfire;
using Hangfire.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.JobSchedule.Hangfire.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHangfireInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IJobScheduler, JobScheduler>();

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




