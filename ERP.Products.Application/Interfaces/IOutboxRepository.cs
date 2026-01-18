using ERP.Products.Domain.Entities;

namespace ERP.Products.Application.Interfaces;

public interface IOutboxRepository
{
    Task<IEnumerable<ProductOutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken ct = default);
    Task InsertAsync(ProductOutboxMessage message, CancellationToken ct = default);
    Task UpdateAsync(ProductOutboxMessage message, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
