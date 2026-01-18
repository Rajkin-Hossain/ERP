using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ERP.Products.Persistance.MongoDb.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public const string CollectionName = "products";

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToCollection(CollectionName);

        builder.HasKey(product => product.Id);
        builder.Property(product => product.Id)
            .HasConversion(id => id.Value, value => new ProductId(value))
            .HasElementName("_id");

        builder.Property(product => product.CategoryId)
            .HasConversion(id => id.Value, value => new CategoryId(value))
            .HasElementName("CategoryId")
            .IsRequired();

        builder.Property(product => product.ProductName)
            .HasConversion(name => name.Value, value => new ProductName(value))
            .HasElementName("ProductName")
            .IsRequired();

        builder.Property(product => product.ImageUrl)
            .HasConversion(url => url.Value, value => new ImageUrl(value))
            .HasElementName("ImageUrl")
            .IsRequired();

        builder.Property(product => product.Price)
            .HasConversion(price => price.Value, value => new Price(value))
            .HasElementName("Price")
            .IsRequired();
    }
}
