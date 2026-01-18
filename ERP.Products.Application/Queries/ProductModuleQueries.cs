using ERP.Products.Application.Result;
using ERP.Products.Application.Contracts;
using MediatR;

namespace ERP.Products.Application.Queries;

public sealed record GetProductsQuery() : IRequest<AppResult<IEnumerable<ProductResult>>>;

