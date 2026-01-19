namespace ERP.Products.Presentation.EndPoints.CreateProduct;

public sealed record CreateProductRequest(
    string Name,
    decimal Price,
    string ImageUrl,
    string CategoryId);
