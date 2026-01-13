using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.ProductModule.Infrastructure.Data.ProductContext.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", schema: "product_schema");

        builder.HasKey(c => c.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                v => v.Value,
                v => new CategoryId(v))
            .ValueGeneratedNever();

        builder.Property(c => c.CategoryName)
            .HasConversion(
                v => v.Value,
                v => new CategoryName(v))
            .HasMaxLength(200)
            .IsRequired();
    }
}
