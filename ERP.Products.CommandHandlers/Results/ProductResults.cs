using ERP.Products.Domain.ValueObjects;

namespace ERP.Products.CommandHandlers.Results;

public sealed record ProductResult(
    ProductId ProductId,
    CategoryId CategoryId,
    ProductName ProductName,
    ImageUrl ImageUrl,
    Price Price
);


