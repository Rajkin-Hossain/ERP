using ERP.Products.Persistance.PgSQL.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Products.Persistance.PgSQL.Configurations.Write;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public const string TableName = "product_outbox";

    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableName, schema: "outbox_schema");

        // Primary key
        builder.HasKey(x => x.Id);

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
            .IsRequired(false);

        // Payload (JSON)
        builder.Property(x => x.Payload)
            .IsRequired();
        // Status
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50); //Pending
    }
}






