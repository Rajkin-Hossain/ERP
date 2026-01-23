namespace ERP.Products.Presentation.CommandEndPoints.UpdateProduct;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price,
    string ImageUrl);
