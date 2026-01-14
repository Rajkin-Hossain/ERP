using ERP.ProductModule.Application.Mappers;
using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Infrastructure.Data.ProductContext;
using ERP.SharedKernal.Enums;
using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.ProductModule.Infrastructure.BackgroundJobs.ProductContext;

public class ProductOutboxJob
{
    private readonly IServiceBus _bus;
    private readonly ProductDbContext _dbContext;

    public ProductOutboxJob(IServiceBus bus, ProductDbContext dbContext)
    {
        _bus = bus;
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        var batch = await _dbContext.Set<ProductOutboxMessage>()
            .AsNoTracking()
            .Where(m => m.Status == OutboxStatus.Pending)
            .ToListAsync(ct);

        if (batch.Count == 0) return;

        foreach (var m in batch)
        {
            try
            {
                var integrationEvent = IntegrationEventMapper.Map(m.EventType, m.Payload);

                await _bus.Publish(integrationEvent, ct);

                m.MarkSent(DateTime.UtcNow);
            }
            catch (Exception)
            {
                m.MarkFailed(DateTime.UtcNow);
            }
        }

        await _dbContext.SaveChangesAsync(ct);
    }
}
