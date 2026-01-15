using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Domain.ValueObjects.Outbox;
using ERP.SharedKernal.Interfaces;

namespace ERP.ProductModule.Application.RepoInterfaces.Read;

public interface IProductOutboxRepository : IRepositoryBase<ProductOutboxMessage, ProductOutboxMessageId> { }