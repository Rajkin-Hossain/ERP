using ERP.Products.Application.Interfaces;
using ERP.Products.Application.Mappers;
using ERP.Products.Application.Queries;
using ERP.Products.Application.Results;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Application.Result;
using MediatR;
namespace ERP.Products.Application.QueryHandlers;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, AppResult<IEnumerable<ProductResult>>>
{
    private readonly IProductReadRepository _repo;
    private readonly IAsyncQueryExecutor _executor;

    public GetProductsQueryHandler(IProductReadRepository repo, IAsyncQueryExecutor executor)
    {
        _repo = repo;
        _executor = executor;
    }

    public async Task<AppResult<IEnumerable<ProductResult>>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var products = _repo.GetConditional();

        var results = await _executor.ToListAsync(
            products.Select(ProductMappers.ToProductToProductResultExpression),
        ct);

        return AppResult<IEnumerable<ProductResult>>.Ok(results);
    }
}







