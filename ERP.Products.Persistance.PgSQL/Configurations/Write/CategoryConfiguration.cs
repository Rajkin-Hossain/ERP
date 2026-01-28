using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Products.Persistance.PgSQL.Configurations.Write;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public const string TableName = "categories";

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TableName, schema: "category_schema");

        builder.HasKey(c => c.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                v => v.Value,
                v => CategoryId.Create(v));

        builder.Property(c => c.CategoryName)
            .HasConversion(
                v => v.Value,
                v => CategoryName.Create(v))
            .HasMaxLength(200)
            .IsRequired();
    }
}






