using ERP.ProductModule.Application.Results;
using ERP.SharedKernal.AppResult;
using MediatR;

namespace ERP.ProductModule.Application.Queries;

public sealed record GetProductsQuery() : IRequest<AppResult<IEnumerable<ProductResults>>>;
