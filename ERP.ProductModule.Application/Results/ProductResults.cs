using ERP.ProductModule.Domain.ValueObjects;

namespace ERP.ProductModule.Application.Results;

public sealed record ProductResult(
    ProductId ProductId,
    CategoryId CategoryId,
    ProductName ProductName,
    ImageUrl ImageUrl,
    Price Price
);