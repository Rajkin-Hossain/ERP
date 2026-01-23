using ERP.Products.Domain.Entities;
using ERP.Shared.Domain.OutboxEntity;
using Microsoft.EntityFrameworkCore;
namespace ERP.Products.Persistance.MongoDb.Data;

public sealed class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<OutboxMessage> ProductOutboxMessages => Set<OutboxMessage>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Mongo EF Core: disable transactions when Mongo server doesn't support them
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}






