using ERP.Products.Domain.ValueObjects;
namespace ERP.Products.Application.Results;

public sealed record ProductResult(
    ProductId ProductId,
    CategoryId CategoryId,
    ProductName ProductName,
    ImageUrl ImageUrl,
    Price Price
);


