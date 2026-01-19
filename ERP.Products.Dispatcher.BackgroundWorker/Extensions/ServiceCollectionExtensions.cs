using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Dispatcher.BackgroundWorker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutboxBackgroundWorker(this IServiceCollection services)
    {
        services.AddHostedService<OutboxBackgroundWorker>();
        return services;
    }
}
