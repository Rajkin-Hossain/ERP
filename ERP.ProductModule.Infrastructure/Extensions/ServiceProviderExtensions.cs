using ERP.ProductModule.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.ProductModule.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task MigrateDbContext(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        await dbContext.Database.MigrateAsync();
    }
}
