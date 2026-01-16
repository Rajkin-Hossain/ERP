using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using MongoDb.Configurations;
using MongoDb.Serializers;
using MongoDB.Bson.Serialization;

namespace ERP.ProductModule.MongoDb.Configurations;

public class ProductConfiguration : MongoDbConfiguration<Product>
{
    protected override void ApplyConfiguration(BsonClassMap<Product> map)
    {
        map.MapIdMember(p => p.Id)
           .SetSerializer(new SingleValueObjectSerializer<ProductId, Guid>(v => new ProductId(v), o => o.Value));

        map.MapMember(p => p.ProductName)
           .SetSerializer(new SingleValueObjectSerializer<ProductName, string>(v => new ProductName(v), o => o.Value));

        map.MapMember(p => p.CategoryId)
           .SetSerializer(new SingleValueObjectSerializer<CategoryId, Guid>(v => new CategoryId(v), o => o.Value));

        map.MapMember(p => p.ImageUrl)
           .SetSerializer(new SingleValueObjectSerializer<ImageUrl, string>(v => new ImageUrl(v), o => o.Value));

        map.MapMember(p => p.Price)
           .SetSerializer(new SingleValueObjectSerializer<Price, decimal>(v => new Price(v), o => o.Value));

        map.UnmapMember(p => p.DomainEvents);
    }
}
