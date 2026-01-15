using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.ProductModule.Domain.Entities.Outbox;
using ERP.ProductModule.Domain.ValueObjects.Outbox;
using ERP.ProductModule.MongoDb.Data;
using MongoDb;

namespace ERP.ProductModule.MongoDb.Repositories;

public class ProductOutboxRepository(ProductReadDbContext dbContext)
    : MongoRepositoryBase<ProductOutboxMessage, ProductOutboxMessageId>(dbContext), IProductOutboxRepository
{
}
