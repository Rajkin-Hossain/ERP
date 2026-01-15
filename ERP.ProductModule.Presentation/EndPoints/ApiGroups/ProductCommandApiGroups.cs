using ERP.ProductModule.Application.Commands;
using ERP.ProductModule.MongoDb.Outbox;
using ERP.ProductModule.Presentation.Extensions;
using Hangfire;
using MediatR;

namespace ERP.ProductModule.Presentation.EndPoints.ApiGroups;

public static class ProductCommandApiGroups
{
    public static void MapProductCommandApiGroups(this RouteGroupBuilder group)
    {
        group.MapPost(string.Empty, async (CreateProductCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            BackgroundJob.Enqueue<ProductOutboxJob>(job => job.ExecuteAsync());

            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Create Product")
        .WithDescription("Creates a product")
        .WithName("CreateProduct");

        group.MapPut("/{productId}", async (string productId, UpdateProductCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            BackgroundJob.Enqueue<ProductOutboxJob>(job => job.ExecuteAsync());

            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Update Product")
        .WithDescription("Update product")
        .WithName("UpdateProduct");
    }
}
