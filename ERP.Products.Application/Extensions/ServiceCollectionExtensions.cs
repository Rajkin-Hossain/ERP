using ERP.Products.Application.Behaviors;
using ERP.Shared.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ERP.Products.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(OutboxDispatchBehavior<,>));
            cfg.AddOpenBehavior(typeof(NonTransactionalBehavior<,>));
            //cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        return services;
    }
}



