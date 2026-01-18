using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Application.Result;

namespace ERP.Products.Application.Commands;

public sealed record CreateProductCommand(string Name, decimal Price, string ImageUrl, string CategoryId)
    : IBaseCommand<AppResult<ProductId>>;

public sealed record UpdateProductCommand(Guid ProductId, string Name, decimal Price, string ImageUrl)
    : IBaseCommand<AppResult<ProductId>>;





