using ERP.Products.Persistance.PgSQL.BaseRepository.DbContexts.ProductDbContext.Write;
using ERP.Shared.Domain.Entities;

namespace ERP.Products.Persistance.PgSQL.Repositories.DbContexts.ProductDbContext.Write;

public class ProductRepository<T, TId>(ProductDbContext dbcontext)
    : PgRepository<T, TId>(dbcontext) where T : Entity<TId> where TId : notnull
{
}







