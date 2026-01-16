using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Domain.ValueObjects.Outbox;
using ERP.ProductModule.MongoDb.Data;
using ERP.ProductModule.MongoDb.Repositories;

namespace ERP.ProductModule.MongoDb.Repositories;

public class ProductOutboxRepository(ProductDbContext dbContext)
    : EfCoreRepositoryBase<ProductOutboxMessage, ProductOutboxMessageId>(dbContext), IProductOutboxRepository
{
}
