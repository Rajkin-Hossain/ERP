using ERP.Shared.Application.Interfaces;

using ERP.Shared.Domain.Entities;

using ERP.Products.Domain.Enums;
using ERP.Products.Persistance.MongoDb.Data;
using Microsoft.EntityFrameworkCore;

using ERP.Products.Domain.Entities;
using ERP.Products.Application.Interfaces;
namespace ERP.Products.Persistance.MongoDb.Repositories;

public class OutboxRepository(ProductDbContext dbcontext) : IOutboxRepository
{
    public async Task<IEnumerable<ProductOutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken ct = default)
    {
        return await dbcontext.ProductOutboxMessages
            .Where(m => m.Status == OutboxStatus.Pending)
            .ToListAsync(ct);
    }

    public async Task InsertAsync(ProductOutboxMessage message, CancellationToken ct = default)
    {
        await dbcontext.ProductOutboxMessages.AddAsync(message, ct);
    }

    public async Task UpdateAsync(ProductOutboxMessage message, CancellationToken ct = default)
    {
        dbcontext.ProductOutboxMessages.Update(message);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await dbcontext.SaveChangesAsync(ct);
    }
}





