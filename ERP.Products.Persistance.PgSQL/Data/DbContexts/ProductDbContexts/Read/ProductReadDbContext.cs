using ERP.Products.Domain.Entities;
using ERP.Products.Persistance.PgSQL.Configurations.DbContexts.ProductDbContext;
using ERP.Shared.Infrastructures.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ERP.Products.Persistance.PgSQL.Data.DbContexts.ProductDbContext.Read;

public sealed class ProductReadDbContext(DbContextOptions<ProductReadDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<OutboxMessage> ProductOutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductOutboxMessageConfiguration());
    }
}






