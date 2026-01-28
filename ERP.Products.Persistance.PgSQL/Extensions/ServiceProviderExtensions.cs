using ERP.Products.Persistance.PgSQL.Data.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Products.Persistance.PgSQL.Extensions;

public static class ServiceProviderExtensions
{
    public static Task MigrateDbContext(this IServiceProvider services, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(services);

        using IServiceScope scope = services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        return dbContext.Database.MigrateAsync(ct);
    }
}

