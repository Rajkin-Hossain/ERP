using ERP.Products.Persistance.PgSQL.Data.DbContexts.ProductDbContext.Write;
using ERP.Shared.Infrastructures.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ERP.Products.Persistance.PgSQL.Storages;

public class OutboxStorage(ProductDbContext dbcontext)
{
    public async Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken ct = default)
    {
        return await dbcontext.ProductOutboxMessages
            .Where(m => m.Status == OutboxStatus.Pending)
            .ToListAsync(ct);
    }

    public async Task InsertAsync(OutboxMessage message, CancellationToken ct = default)
    {
        await dbcontext.ProductOutboxMessages.AddAsync(message, ct);
    }

    public async Task UpdateAsync(OutboxMessage message, CancellationToken ct = default)
    {
        dbcontext.ProductOutboxMessages.Update(message);
        await Task.CompletedTask;
    }

    public async Task MarkSent(OutboxMessage message, CancellationToken ct = default)
    {
        message.MarkSent(DateTime.UtcNow);
        dbcontext.ProductOutboxMessages.Update(message);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await dbcontext.SaveChangesAsync(ct);
    }
}





