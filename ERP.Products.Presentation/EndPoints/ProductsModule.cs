using ERP.Products.Presentation.EndPoints.CreateProduct;
using ERP.Products.Presentation.EndPoints.GetProducts;
using ERP.Products.Presentation.EndPoints.UpdateProduct;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.EndPoints;

public static class ProductsModule
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/product")
            .WithTags("Product");

        group.MapCreateProductEndpoint();
        group.MapUpdateProductEndpoint();
        group.MapGetProductsEndpoint();
    }
}
