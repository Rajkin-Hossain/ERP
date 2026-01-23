using ERP.Products.Application.Queries;
using ERP.Shared.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.QueryEndPoints.GetProducts;

public static class GetProductsEndpoint
{
    public static void MapGetProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(string.Empty, async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetProductsQuery(), ct);
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Get Products")
        .WithDescription("Get product")
        .WithName("GetProducts");
    }
}
