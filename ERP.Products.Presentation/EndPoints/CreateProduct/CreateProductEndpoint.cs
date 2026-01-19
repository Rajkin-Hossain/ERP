using ERP.Products.Application.Commands;
using ERP.Shared.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.EndPoints.CreateProduct;

public static class CreateProductEndpoint
{
    public static void MapCreateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(string.Empty, async (CreateProductRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateProductCommand(
                request.Name,
                request.Price,
                request.ImageUrl,
                request.CategoryId);

            var result = await mediator.Send(command, ct);
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Create Product")
        .WithDescription("Creates a product")
        .WithName("CreateProduct");
    }
}
