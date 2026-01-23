using ERP.Products.Persistance.MongoDb.BaseRepository;
using ERP.Products.Persistance.MongoDb.Data;
using ERP.Shared.Domain.Entities;

namespace ERP.Products.Persistance.MongoDb.Repositories;

public class ProductRepository<T, TId>(ProductDbContext dbcontext)
    : MongoRepository<T, TId>(dbcontext) where T : Entity<TId> where TId : notnull
{
}







