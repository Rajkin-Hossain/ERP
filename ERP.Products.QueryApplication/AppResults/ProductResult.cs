namespace ERP.Products.QueryApplication.AppResults;

public sealed record ProductResult(
    Guid ProductId,
    Guid CategoryId,
    string ProductName,
    string ImageUrl,
    decimal Price
);
