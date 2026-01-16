using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.ProductModule.MongoDb.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public const string CollectionName = "products";

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToCollection(CollectionName);

        builder.HasKey(product => product.Id);
        builder.Property(product => product.Id)
            .HasConversion(id => id.Value, value => new ProductId(value));

        builder.Property(product => product.ProductName)
            .HasConversion(name => name.Value, value => new ProductName(value))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(product => product.CategoryId)
            .HasConversion(id => id.Value, value => new CategoryId(value))
            .IsRequired();

        builder.Property(product => product.ImageUrl)
            .HasConversion(url => url.Value, value => new ImageUrl(value))
            .IsRequired();

        builder.Property(product => product.Price)
            .HasConversion(price => price.Value, value => new Price(value))
            .IsRequired();
    }
}
