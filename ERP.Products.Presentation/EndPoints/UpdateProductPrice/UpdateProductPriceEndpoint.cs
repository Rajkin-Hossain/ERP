using ERP.Products.Application.Commands;
using ERP.Shared.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ERP.Products.Presentation.EndPoints.UpdateProductPrice;

public static class UpdateProductPriceEndpoint
{
    public static void MapUpdateProductPriceEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/{productId:guid}/price", async (Guid productId, UpdateProductPriceRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateProductPriceCommand(productId, request.NewPrice);
            var result = await mediator.Send(command, ct);
            return result.ToHttpResult();
        })
        .ProducesStandardApiResponses()
        .WithSummary("Update Product Price")
        .WithDescription("Updates the price of a product")
        .WithName("UpdateProductPrice");
    }
}
