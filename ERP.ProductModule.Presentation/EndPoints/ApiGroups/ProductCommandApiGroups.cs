using ERP.ProductModule.Presentation.Extensions;
using ERP.Products.Application.Commands;
using ERP.Products.Presentation.Extensions;
using MediatR;

namespace ERP.Products.Presentation.EndPoints.ApiGroups;

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
