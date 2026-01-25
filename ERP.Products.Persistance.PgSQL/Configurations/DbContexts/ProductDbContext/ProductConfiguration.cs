using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Products.Persistance.PgSQL.Configurations.DbContexts.ProductDbContext;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public const string TableName = "products";

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableName, schema: "product_schema");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                v => v.Value,
                v => ProductId.Create(v))
            .ValueGeneratedNever();

        builder.Property(p => p.ProductName)
            .HasConversion(
                v => v.Value,
                v => ProductName.Create(v))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.CategoryId)
            .HasConversion(
                v => v.Value,
                v => CategoryId.Create(v))
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .HasConversion(
                v => v.Value,
                v => ImageUrl.Create(v))
            .IsRequired(false);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        builder.Property(p => p.Price)
            .HasConversion(
                v => v.Value,
                v => Price.Create(v))
            .IsRequired(false);
    }
}






