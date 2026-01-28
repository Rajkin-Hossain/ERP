using ERP.Products.Domain.Entities;
using ERP.Products.Persistance.PgSQL.Configurations.Write;
using Microsoft.EntityFrameworkCore;

namespace ERP.Products.Persistance.PgSQL.Data.Read;

public sealed class ProductReadDbContext(DbContextOptions<ProductReadDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
    }
}






