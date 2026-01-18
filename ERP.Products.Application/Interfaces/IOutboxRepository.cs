using ERP.Shared.Domain.OutboxEntity;

namespace ERP.Products.Application.Interfaces;

public interface IOutboxRepository
{
    Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken ct = default);
    Task MarkSent(OutboxMessage message, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}






