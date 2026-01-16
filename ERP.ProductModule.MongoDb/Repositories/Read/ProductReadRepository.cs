using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.MongoDb.Data;
using ERP.ProductModule.MongoDb.Repositories;

namespace ERP.ProductModule.MongoDb.Repositories.Read;

public class ProductReadRepository(ProductDbContext dbcontext)
    : EfCoreRepositoryBase<Product, ProductId>(dbcontext), IProductReadRepository
{
}
