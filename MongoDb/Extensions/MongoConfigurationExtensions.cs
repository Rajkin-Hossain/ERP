using Microsoft.Extensions.DependencyInjection;
using MongoDb.Conventions;
using MongoDb.Configurations;
using System.Reflection;

namespace MongoDb.Extensions;

public static class MongoConfigurationExtensions
{
    /// <summary>
    /// Scans the specified assemblies for <see cref="IMongoDbConfiguration"/> implementations and executes their configuration logic.
    /// This should be called at application startup before any MongoDB operations.
    /// </summary>
    /// <param name="services">The service collection (used for chaining).</param>
    /// <param name="assemblies">The assemblies to scan.</param>
    public static IServiceCollection RegisterMongoConfigurations(this IServiceCollection services, params Assembly[] assemblies)
    {
        MongoDbConventions.Register();

        foreach (var assembly in assemblies)
        {
            var configTypes = assembly.GetTypes()
                .Where(t => typeof(IMongoDbConfiguration).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in configTypes)
            {
                if (Activator.CreateInstance(type) is IMongoDbConfiguration config)
                {
                    config.Configure();
                }
            }
        }

        return services;
    }
}
