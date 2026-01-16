using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Domain.ValueObjects.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ERP.ProductModule.MongoDb.Configurations;

public sealed class ProductOutboxMessageConfiguration : IEntityTypeConfiguration<ProductOutboxMessage>
{
    public const string CollectionName = "product_outbox";

    public void Configure(EntityTypeBuilder<ProductOutboxMessage> builder)
    {
        builder.ToCollection(CollectionName);

        builder.HasKey(message => message.Id);
        builder.Property(message => message.Id)
            .HasConversion(id => id.Value, value => new ProductOutboxMessageId(value))
            .HasElementName("_id");

        builder.Property(message => message.EventType)
            .HasElementName("EventType")
            .IsRequired();

        builder.Property(message => message.AggregateId)
            .HasElementName("AggregateId")
            .IsRequired();

        builder.Property(message => message.Payload)
            .HasElementName("Payload")
            .IsRequired();

        builder.Property(message => message.Status)
            .HasConversion<int>()
            .HasElementName("Status")
            .IsRequired();

        builder.Property(message => message.OccurredOnUtc)
            .HasElementName("OccurredOnUtc")
            .IsRequired();

        builder.Property(message => message.RetryCount)
            .HasElementName("RetryCount")
            .IsRequired();
    }
}
