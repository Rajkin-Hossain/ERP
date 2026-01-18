using ERP.Products.Presentation.Extensions;
using ERP.Products.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.EndPoints.ApiGroups;

public static class ProductQueryApiGroups
{
    public static void MapProductQueryApiGroups(this RouteGroupBuilder group)
    {
        group.MapGet(string.Empty, async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProductsQuery(), ct);
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Get Products")
        .WithDescription("Get product")
        .WithName("GetProducts");
    }
}
