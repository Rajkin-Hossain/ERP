using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Products.QueryApplication.AppResults;
using ERP.Products.QueryApplication.Mappers;
using ERP.Shared.Application.AppRecords;
using ERP.Shared.Application.Interfaces;

namespace ERP.Products.QueryApplication.Services;

public sealed class ProductService(IReadRepository<Product, ProductId> repo, IQueryExecutor asyncQueryExecutor)
{
    private readonly IReadRepository<Product, ProductId> _repo = repo;
    private readonly IQueryExecutor _asyncQueryExecutor = asyncQueryExecutor;

    public async Task<AppResult<IEnumerable<ProductResult>>> GetProductsAsync(CancellationToken ct)
    {
        var products = _repo.GetConditional();

        var results = await _asyncQueryExecutor.ToListAsync(
            products.Select(ProductMappers.ToProductToProductResultExpression),
        ct);

        return AppResult<IEnumerable<ProductResult>>.Ok(results);
    }
}