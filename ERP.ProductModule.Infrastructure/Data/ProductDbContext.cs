using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.ProductModule.Infrastructure.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("product_schema");

        builder.Entity<Product>(product =>
        {
            product.ToTable("products");

            product.HasKey(p => p.Id);
            product.Property(p => p.Id).ValueGeneratedNever();

            product.Property(p => p.ProductName)
                .HasConversion(
                    v => v.Value,
                    v => new ProductName(v))
                .HasMaxLength(200)
                .IsRequired();

            product.Property(p => p.CategoryId)
                .HasConversion(
                    v => v.Value,
                    v => new CategoryId(v))
                .IsRequired();

            product.Property(p => p.ImageUrl)
                .HasConversion(
                    v => v.Value,
                    v => new ImageUrl(v))
                .IsRequired(false);

            product.OwnsOne(p => p.Price, price =>
            {
                price.Property(p => p.Amount)
                    .HasColumnName("price_amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                price.Property(p => p.Currency)
                    .HasColumnName("price_currency")
                    .HasMaxLength(10)
                    .IsRequired();
            });

            product.Ignore(p => p.DomainEvents);
        });

        builder.Entity<Category>(category =>
        {
            category.ToTable("categories");

            category.HasKey(c => c.Id);
            category.Property(c => c.Id).ValueGeneratedNever();

            category.Property(c => c.CategoryName)
                .HasConversion(
                    v => v.Value,
                    v => new CategoryName(v))
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.Ignore<IDomainEvent>();
    }
}
