using ERP.Products.Persistance.MongoDb;
using ERP.Shared.Application.Interfaces;

using ERP.Shared.Domain.Entities;

using ERP.Products.Domain.ValueObjects;
using ERP.Products.Persistance.MongoDb.Data;

using ERP.Products.Domain.Entities;
using ERP.Products.Application.Interfaces;
namespace ERP.Products.Persistance.MongoDb.Repositories;

public class ProductReadRepository(ProductDbContext dbcontext)
    : MongoRepositoryBase<Product, ProductId>(dbcontext), IProductReadRepository
{
}







