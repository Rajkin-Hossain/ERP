using ERP.Products.Persistance.PgSQL.BaseRepository.DbContexts.ProductDbContexts.Read;
using ERP.Products.Persistance.PgSQL.Data.DbContexts.ProductDbContexts.Read;
using ERP.Shared.Domain.Entities;

namespace ERP.Products.Persistance.PgSQL.Repositories.DbContexts.ProductDbContexts.Read;

public class ProductReadRepository<T, TId>(ProductReadDbContext dbcontext)
    : PgReadRepository<T, TId>(dbcontext) where T : Entity<TId> where TId : notnull
{
}







