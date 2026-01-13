using EFCore;
using ERP.ProductModule.Application.RepoInterfaces;
using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.ProductModule.Infrastructure.Data.ProductContext;

namespace ERP.ProductModule.Infrastructure.Repositories.ProductContext;

public class ProductRepository(ProductDbContext context)
    : RepositoryBase<Product, ProductId>(context), IProductRepository
{
}
