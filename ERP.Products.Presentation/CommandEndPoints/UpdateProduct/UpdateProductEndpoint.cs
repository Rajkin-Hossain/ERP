using ERP.Products.Application.Commands;
using ERP.Shared.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.CommandEndPoints.UpdateProduct;

public static class UpdateProductEndpoint
{
    public static void MapUpdateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{productId:guid}", async (Guid productId, UpdateProductRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new UpdateProductCommand(
                productId,
                request.Name,
                request.Price,
                request.ImageUrl);

            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Update Product")
        .WithDescription("Update product")
        .WithName("UpdateProduct");
    }
}
