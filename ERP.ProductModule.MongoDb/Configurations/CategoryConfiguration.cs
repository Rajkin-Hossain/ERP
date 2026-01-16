using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using MongoDb.Configurations;
using MongoDb.Serializers;
using MongoDB.Bson.Serialization;

namespace ERP.ProductModule.MongoDb.Configurations;

public sealed class CategoryConfiguration : MongoDbConfiguration<Category>
{
    protected override void ApplyConfiguration(BsonClassMap<Category> map)
    {
        map.MapIdMember(c => c.Id)
           .SetSerializer(new SingleValueObjectSerializer<CategoryId, Guid>(v => new CategoryId(v), o => o.Value));

        map.MapMember(c => c.CategoryName)
           .SetSerializer(new SingleValueObjectSerializer<CategoryName, string>(v => new CategoryName(v), o => o.Value));
    }
}
