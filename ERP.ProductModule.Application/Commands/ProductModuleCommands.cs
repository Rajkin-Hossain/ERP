using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.AppResult;
using MediatR;

namespace ERP.ProductModule.Application.Commands;

public sealed record CreateProductCommand(string Name, decimal Price, string ImageUrl, Guid CategoryId) : IRequest<AppResult<ProductId>>;
public sealed record UpdateProductCommand(Guid ProductId, string Name, decimal Price, string ImageUrl) : IRequest<AppResult<ProductId>>;