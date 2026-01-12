using Microsoft.EntityFrameworkCore;

namespace ERP.Product.Infrastructure.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("product_schema");
    }
}
