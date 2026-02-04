using ERP.Products.QueryApplication.Services;
using ERP.Shared.Presentation.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ERP.Products.QueryApi.EndPoints;

public class GetProductsEndpoint(ProductService productService) : EndpointWithoutRequest<IResult>
{
    private readonly ProductService _service = productService;

    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();

        // OpenAPI Metadata
        Description(x => x
            .WithSummary("Get Product")
            .WithDescription("Get all products")
            .WithName("GetProduct")
            .ProducesStandardReadApiResponses()
        );
    }

    public override async Task<IResult> ExecuteAsync(CancellationToken ct)
    {
        var result = await _service.GetProductsAsync(ct);

        return result.ToApiResponse();
    }
}
