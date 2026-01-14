namespace ERP.ProductModule.Infrastructure.Data.ProductContext.Configurations.OutBox;

using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Domain.ValueObjects.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ProductOutboxConfiguration
    : IEntityTypeConfiguration<ProductOutboxMessage>
{
    public void Configure(EntityTypeBuilder<ProductOutboxMessage> builder)
    {
        builder.ToTable("ProductOutboxMessages", schema: "outbox_schema");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                v => v.Value,          // OutboxMessageId -> Guid
                v => new ProductOutboxMessageId(v)) // Guid -> OutboxMessageId
            .ValueGeneratedNever();

        // Event type
        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(300);

        // Aggregate Id
        builder.Property(x => x.AggregateId)
            .IsRequired();

        // Occurred time
        builder.Property(x => x.OccurredOnUtc)
            .IsRequired();

        // Published time (nullable until published)
        builder.Property(x => x.PublishedOnUtc)
            .IsRequired();

        // Payload (JSON)
        builder.Property(x => x.Payload)
            .IsRequired();
        // Status
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50); //Pending
    }
}