using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Result;
using MediatR;
namespace ERP.Products.Application.Commands;

public sealed record CreateProductCommand(string Name, decimal Price, string ImageUrl, string CategoryId) : IRequest<AppResult<ProductId>>;

public sealed record UpdateProductCommand(string ProductId, string Name, decimal Price, string ImageUrl) : IRequest<AppResult<ProductId>>;





