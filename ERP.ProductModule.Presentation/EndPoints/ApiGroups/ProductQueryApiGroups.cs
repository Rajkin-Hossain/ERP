using ERP.ProductModule.Application.Queries;
using ERP.ProductModule.Presentation.Extensions;
using MediatR;

namespace ERP.ProductModule.Presentation.EndPoints.ApiGroups;

public static class ProductQueryApiGroups
{
    public static void MapProductQueryApiGroups(this RouteGroupBuilder group)
    {
        group.MapGet(string.Empty, async (GetProductsQuery query, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(query, ct);
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Get Products")
        .WithDescription("Get product")
        .WithName("GetProducts");
    }
}