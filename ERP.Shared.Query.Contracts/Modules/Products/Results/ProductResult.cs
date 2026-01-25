using ERP.Shared.Query.Contracts.Interfaces;

namespace ERP.Shared.Query.Contracts.Modules.Products.Results;

public sealed record ProductResult(
    Guid ProductId,
    Guid CategoryId,
    string ProductName,
    string ImageUrl,
    decimal Price
) : IResult;
