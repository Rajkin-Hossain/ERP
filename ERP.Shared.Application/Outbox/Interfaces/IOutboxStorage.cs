using ERP.Shared.Application.Outbox.Models;

namespace ERP.Shared.Application.Outbox.Interfaces;

public interface IOutboxStorage
{
    Task<IEnumerable<OutboxEnvelope>> GetUnprocessedMessagesAsync(CancellationToken ct = default);
    Task MarkSent(OutboxEnvelope message, CancellationToken ct = default);
    Task MarkFailed(OutboxEnvelope message, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
