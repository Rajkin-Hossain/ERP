using BuildingBlocks.Persistence.MongoDb.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BuildingBlocks.Persistence.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoServices<T>(this IServiceCollection services, 
        IConfiguration configuration) where T : DbContext
    {

        var section = configuration.GetSection(MongoOptions.SectionName);
        services.Configure<MongoOptions>(section);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(section.Get<MongoOptions>()?.ConnectionString));

        services.AddDbContext<T>((provider, options) =>
        {
            var mongoOptions = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoOptions>>().Value;
            var client = provider.GetRequiredService<IMongoClient>();
            options.UseMongoDB(client, mongoOptions.WriteDatabaseName);
        });

        return services;
    }
}
