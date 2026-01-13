using ERP.ProductModule.Presentation.EndPoints.ApiGroups;

namespace ERP.ProductModule.Presentation.EndPoints;

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