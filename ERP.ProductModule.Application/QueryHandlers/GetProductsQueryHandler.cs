using ERP.ProductModule.Application.Mappers;
using ERP.ProductModule.Application.Queries;
using ERP.ProductModule.Application.RepoInterfaces.Read;
using ERP.ProductModule.Application.Results;
using ERP.SharedKernal.AppResult;
using ERP.SharedKernal.Interfaces;
using MediatR;

namespace ERP.ProductModule.Application.QueryHandlers;

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
