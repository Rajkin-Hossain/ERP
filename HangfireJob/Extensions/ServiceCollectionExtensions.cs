using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HangfireJob.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHangfireInfrastructure(
        this IServiceCollection services)
        {
            services.AddHangfire((sp, cfg) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var redisCs = configuration.GetConnectionString("cache");

                cfg.UseSimpleAssemblyNameTypeSerializer()
                   .UseRecommendedSerializerSettings()
                   .UseRedisStorage(redisCs, new RedisStorageOptions
                   {
                       Prefix = "hangfire:",
                       Db = 2, // separate DB index for jobs
                       InvisibilityTimeout = TimeSpan.FromMinutes(5)
                   });
            });

        services.AddHangfireServer();

        return services;
    }
}
