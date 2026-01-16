using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.Entities;
using MongoDb.Configurations;
using MongoDb.Serializers;
using MongoDB.Bson.Serialization;

namespace ERP.ProductModule.MongoDb.Configurations;

public class ProductConfiguration : MongoDbConfiguration<Product>
{
    public override void Configure()
    {
        // Explicitly map the base class to handle the inherited Id property
        if (!BsonClassMap.IsClassMapRegistered(typeof(Entity<ProductId>)))
        {
            BsonClassMap.RegisterClassMap<Entity<ProductId>>(map =>
            {
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(c => c.Id)
                   .SetSerializer(new SingleValueObjectSerializer<ProductId, Guid>(v => new ProductId(v), o => o.Value));
            });
        }

        base.Configure();
    }

    protected override void Configure(BsonClassMap<Product> map)
    {
        map.SetIgnoreExtraElements(true);

        // Id is mapped in the base class Entity<ProductId>

        map.MapMember(p => p.ProductName)
           .SetSerializer(new SingleValueObjectSerializer<ProductName, string>(v => new ProductName(v), o => o.Value));

        map.MapMember(p => p.CategoryId)
           .SetSerializer(new SingleValueObjectSerializer<CategoryId, Guid>(v => new CategoryId(v), o => o.Value));

        map.MapMember(p => p.ImageUrl)
           .SetSerializer(new SingleValueObjectSerializer<ImageUrl, string>(v => new ImageUrl(v), o => o.Value));

        map.MapMember(p => p.Price)
           .SetSerializer(new SingleValueObjectSerializer<Price, decimal>(v => new Price(v), o => o.Value));
    }
}
