using ERP.Products.Persistance.PgSQL.Data.Write;
using ERP.Products.Persistance.PgSQL.Outbox.Models;
using ERP.Shared.Application.Outbox.Interfaces;
using ERP.Shared.Application.Outbox.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Products.Persistance.PgSQL.Outbox.Storages;

public sealed class OutboxStorage(ProductDbContext dbcontext) : IOutboxStorage
{
    private readonly ProductDbContext _dbContext = dbcontext;

    public async Task<IEnumerable<OutboxEnvelope>> GetUnprocessedMessagesAsync(CancellationToken ct = default)
    {
        return await _dbContext.OutboxMessages
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
        var entity = await GetEntityAsync(message, ct);
        entity.MarkSent(DateTime.UtcNow);
    }

    public async Task MarkFailed(OutboxEnvelope message, CancellationToken ct = default)
    {
        var entity = await GetEntityAsync(message, ct);
        entity.MarkFailed(DateTime.UtcNow);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _dbContext.SaveChangesAsync(ct);

    private async Task<OutboxMessage> GetEntityAsync(OutboxEnvelope message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);

        var entity = await _dbContext.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == message.Id, ct);

        return entity ?? throw new InvalidOperationException(
            $"Outbox message '{message.Id}' was not found.");
    }
}





