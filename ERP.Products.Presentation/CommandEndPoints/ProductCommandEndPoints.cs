using ERP.Products.Presentation.CommandEndPoints.CreateProduct;
using ERP.Products.Presentation.CommandEndPoints.UpdateProduct;
using ERP.Products.Presentation.CommandEndPoints.UpdateProductPrice;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.CommandEndPoints;

public static class ProductCommandEndPoints
{
    public static void MapProductCommandEndpoints(this IEndpointRouteBuilder app)
    {
        var commandGroup = app.MapGroup("/products")
            .WithTags("Command Product");

        commandGroup.MapCreateProductEndpoint();
        commandGroup.MapUpdateProductEndpoint();
        commandGroup.MapUpdateProductPriceEndpoint();
    }
}
