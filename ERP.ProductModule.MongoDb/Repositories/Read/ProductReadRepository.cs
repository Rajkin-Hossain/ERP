using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.MongoDb.Data;
using MongoDb;

namespace ERP.ProductModule.MongoDb.Repositories.Read;

public class ProductReadRepository(ProductReadDbContext dbcontext)
    : MongoRepositoryBase<Product, ProductId>(dbcontext), IProductReadRepository
{
}
