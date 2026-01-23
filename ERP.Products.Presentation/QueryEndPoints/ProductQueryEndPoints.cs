using ERP.Products.Presentation.QueryEndPoints.GetProducts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.QueryEndPoints;

public static class ProductQueryEndPoints
{
    public static void MapProductQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var queryGroup = app.MapGroup("/products")
            .WithTags("Query Product");

        queryGroup.MapGetProductsEndpoint();
    }
}
