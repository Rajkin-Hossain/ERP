using ERP.ProductModule.Application.Queries;
using ERP.ProductModule.Presentation.Extensions;
using MediatR;

namespace ERP.ProductModule.Presentation.EndPoints.ApiGroups;

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
