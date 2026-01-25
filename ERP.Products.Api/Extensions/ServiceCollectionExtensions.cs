using ERP.Products.Dispatcher.Hangfire.Extensions;
using ERP.Products.Persistance.PgSQL.Extensions;

namespace ERP.Products.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPgInfrastructure(configuration);
        services.AddHangfireInfrastructure();
        //services.AddOutboxBackgroundWorker();

        return services;
    }
}




