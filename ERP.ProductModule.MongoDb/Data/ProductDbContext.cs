using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.Entities.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ERP.ProductModule.MongoDb.Data;

public sealed class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductOutboxMessage> ProductOutboxMessages => Set<ProductOutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
