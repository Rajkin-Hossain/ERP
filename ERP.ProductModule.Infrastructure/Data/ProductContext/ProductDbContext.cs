using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Infrastructure.Data.ProductContext.Configurations;
using ERP.ProductModule.Infrastructure.Data.ProductContext.Configurations.OutBox;
using ERP.ProductModule.Infrastructure.Data.ProductContext.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ERP.ProductModule.Infrastructure.Data.ProductContext;

public class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductOutboxMessage> OutboxMessages => Set<ProductOutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductOutboxConfiguration());
    }
}
