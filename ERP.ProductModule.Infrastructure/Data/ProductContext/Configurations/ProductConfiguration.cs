using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.ProductModule.Infrastructure.Data.ProductContext.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", schema: "product_schema");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                v => v.Value,          // ProductId -> Guid
                v => new ProductId(v)) // Guid -> ProductId
            .ValueGeneratedNever();

        builder.Property(p => p.ProductName)
            .HasConversion(
                v => v.Value,
                v => new ProductName(v))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.CategoryId)
            .HasConversion(
                v => v.Value,
                v => new CategoryId(v))
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .HasConversion(
                v => v.Value,
                v => new ImageUrl(v))
            .IsRequired(false);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        builder.Property(p => p.Price)
            .HasConversion(
                v => v.Value,
                v => new Price(v))
            .IsRequired(false);
    }
}
