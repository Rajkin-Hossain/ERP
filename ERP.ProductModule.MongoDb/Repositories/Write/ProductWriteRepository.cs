using ERP.ProductModule.Application.RepoInterfaces.Write;
using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.MongoDb.Data;
using ERP.ProductModule.MongoDb.Repositories;

namespace ERP.ProductModule.MongoDb.Repositories.Write;

public class ProductWriteRepository(ProductDbContext dbcontext)
    : EfCoreRepositoryBase<Product, ProductId>(dbcontext), IProductWriteRepository
{
}
