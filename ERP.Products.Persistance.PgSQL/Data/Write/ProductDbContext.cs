using ERP.Products.Domain.Entities;
using ERP.Shared.Infrastructures.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ERP.Products.Persistance.PgSQL.Data.Write;

public sealed class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}
