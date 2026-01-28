using ERP.Products.Persistance.PgSQL.Data.Write;
using ERP.Shared.Application.Abstractions.Outbox;
using ERP.Shared.Infrastructures.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ERP.Products.Persistance.PgSQL.Storages.Outbox;

public sealed class OutboxStorage(ProductDbContext dbcontext) : IOutboxStorage
{
    public async Task<IEnumerable<OutboxEnvelope>> GetUnprocessedMessagesAsync(CancellationToken ct = default)
    {
        return await dbcontext.ProductOutboxMessages
            .AsNoTracking()
            .Where(m => m.Status == OutboxStatus.Pending)
            .OrderBy(m => m.OccurredOnUtc)
            .Select(m => new OutboxEnvelope
            {
                Id = m.Id,
                EventType = m.EventType,
                AggregateId = m.AggregateId,
                OccurredOnUtc = m.OccurredOnUtc,
                PublishedOnUtc = m.PublishedOnUtc,
                Payload = m.Payload,
                Status = (int)m.Status,
                RetryCount = m.RetryCount
            })
            .ToListAsync(ct);
    }

    public async Task MarkSent(OutboxEnvelope message, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        var entity = await dbcontext.ProductOutboxMessages
            .FirstOrDefaultAsync(x => x.Id == message.Id, ct);

        ArgumentNullException.ThrowIfNull(entity);

        entity.MarkSent(DateTime.UtcNow);
    }

    public async Task MarkFailed(OutboxEnvelope message, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        var entity = await dbcontext.ProductOutboxMessages
            .FirstOrDefaultAsync(x => x.Id == message.Id, ct);

        ArgumentNullException.ThrowIfNull(entity);

        entity.MarkFailed(DateTime.UtcNow);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => dbcontext.SaveChangesAsync(ct);
}





