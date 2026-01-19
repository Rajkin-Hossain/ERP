using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Products.Persistance.MongoDb.BaseRepository;
using ERP.Products.Persistance.MongoDb.Data;
namespace ERP.Products.Persistance.MongoDb.Repositories;

public class ProductWriteRepository(ProductDbContext dbcontext)
    : MongoRepositoryBase<Product, ProductId>(dbcontext), IProductWriteRepository
{
}







