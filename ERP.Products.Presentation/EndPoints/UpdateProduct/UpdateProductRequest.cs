namespace ERP.Products.Presentation.EndPoints.UpdateProduct;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price,
    string ImageUrl);
