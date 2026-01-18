using ERP.Shared.Application.Result;
using ERP.Products.Application.Contracts;
using MediatR;

using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
namespace ERP.Products.Application.Queries;

public sealed record GetProductsQuery() : IRequest<AppResult<IEnumerable<ProductResult>>>;






