using ERP.Products.Application.Contracts;
using ERP.Shared.Application.Result;
using MediatR;
namespace ERP.Products.Application.Queries;

public sealed record GetProductsQuery() : IRequest<AppResult<IEnumerable<ProductResult>>>;






