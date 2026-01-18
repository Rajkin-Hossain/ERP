using ERP.Products.Dispatcher.Hangfire.Extensions;
using ERP.Products.Messaging.RabbitMQ.Extensions;
using ERP.Products.Persistance.MongoDb.Extensions;
using ERP.Products.Presentation.Extensions;

namespace ERP.Products.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPresentationServices(configuration);

        services.AddMongoInfrastructure(configuration);
        services.AddMessageBusInfrastructure();
        services.AddHangfireInfrastructure();

        return services;
    }
}




