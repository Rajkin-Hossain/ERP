using ERP.ProductModule.Application.RepoInterfaces.Write;
using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.MongoDb.Data;
using MongoDb;

namespace ERP.ProductModule.MongoDb.Repositories.Write;

public class ProductWriteRepository(ProductWriteDbContext dbcontext)
    : MongoRepositoryBase<Product, ProductId>(dbcontext), IProductWriteRepository
{
}
