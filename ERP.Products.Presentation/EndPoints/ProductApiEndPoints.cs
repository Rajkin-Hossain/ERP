using ERP.Products.Presentation.EndPoints.ApiGroups;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace ERP.Products.Presentation.EndPoints;

public static class ProductApiEndPoints
{
    public static void MapProductApiEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/product")
            .WithTags("Product");

        group.MapProductCommandApiGroups();
        group.MapProductQueryApiGroups();
    }
}


