using ERP.Products.Application.Commands;
using ERP.Products.Application.Interfaces;
using ERP.Products.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.EndPoints.ApiGroups;

public static class ProductCommandApiGroups
{
    public static void MapProductCommandApiGroups(this RouteGroupBuilder group)
    {
        group.MapPost(string.Empty, async (CreateProductCommand cmd, 
            IMediator mediator, 
            IOutboxDispatchTrigger outboxDispatchTrigger, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            outboxDispatchTrigger.EnqueueJob();
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Create Product")
        .WithDescription("Creates a product")
        .WithName("CreateProduct");

        group.MapPut("/{productId}", async (string productId, UpdateProductCommand cmd,
            IOutboxDispatchTrigger outboxDispatchTrigger,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            outboxDispatchTrigger.EnqueueJob();
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Update Product")
        .WithDescription("Update product")
        .WithName("UpdateProduct");
    }
}
