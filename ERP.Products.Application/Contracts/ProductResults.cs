using ERP.Products.Domain.ValueObjects;

namespace ERP.Products.Application.Contracts;

public sealed record ProductResult(
    ProductId ProductId,
    CategoryId CategoryId,
    ProductName ProductName,
    ImageUrl ImageUrl,
    Price Price
);