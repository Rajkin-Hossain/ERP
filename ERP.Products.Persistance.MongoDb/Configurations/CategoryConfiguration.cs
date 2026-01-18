using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
namespace ERP.Products.Persistance.MongoDb.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public const string CollectionName = "categories";

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToCollection(CollectionName);

        builder.HasKey(category => category.Id);
        builder.Property(category => category.Id)
            .HasConversion(id => id.Value, value => new CategoryId(value))
            .HasElementName("_id");

        builder.Property(category => category.CategoryName)
            .HasConversion(name => name.Value, value => new CategoryName(value))
            .HasElementName("CategoryName")
            .IsRequired();
    }
}






