namespace ERP.Products.Presentation.CommandEndPoints.CreateProduct;

public sealed record CreateProductRequest(
    string Name,
    decimal Price,
    string ImageUrl,
    string CategoryId);
