using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Products.QueryHandler.Mappers;
using ERP.Shared.Application.Abstractions.Interfaces;
using ERP.Shared.Kernel.Result;
using ERP.Shared.Query.Contracts.Modules.Products.AppResult;

namespace ERP.Products.QueryHandler.QueryHandlers;

public sealed record GetProductQueryHandler(IReadRepository<Product, ProductId> repo, IQueryExecutor asyncQueryExecutor)
{
    private readonly IReadRepository<Product, ProductId> _repo = repo;
    private readonly IQueryExecutor _asyncQueryExecutor = asyncQueryExecutor;

    public async Task<AppResult<IEnumerable<ProductResult>>> Handle(GetProductQueryHandler query, CancellationToken ct)
    {
        var products = _repo.GetConditional();

        var results = await _asyncQueryExecutor.ToListAsync(
            products.Select(ProductMappers.ToProductToProductResultExpression),
        ct);

        return AppResult<IEnumerable<ProductResult>>.Ok(results);
    }
}
