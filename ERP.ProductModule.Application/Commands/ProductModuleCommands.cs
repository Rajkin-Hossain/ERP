using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.AppResult;
using MediatR;

namespace ERP.ProductModule.Application.Commands;

public sealed record CreateProductCommand(string Name, decimal Price, string ImageUrl, string CategoryId) : IRequest<AppResult<ProductId>>;
public sealed record UpdateProductCommand(string ProductId, string Name, decimal Price, string ImageUrl) : IRequest<AppResult<ProductId>>;