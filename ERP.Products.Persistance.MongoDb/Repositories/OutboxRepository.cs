using ERP.Products.Application.Interfaces;
using ERP.Products.Persistance.MongoDb.Data;
using ERP.Shared.Domain.Enums;
using ERP.Shared.Domain.OutboxEntity;
using Microsoft.EntityFrameworkCore;
namespace ERP.Products.Persistance.MongoDb.Repositories;

public class OutboxRepository(ProductDbContext dbcontext) : IOutboxRepository
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





