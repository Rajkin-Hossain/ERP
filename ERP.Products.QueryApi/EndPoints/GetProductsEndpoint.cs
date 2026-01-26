using ERP.Shared.Kernel.Result;
using ERP.Shared.Presentation.Extensions;
using ERP.Shared.Query.Contracts.Modules.Products.AppResult;
using ERP.Shared.Query.Contracts.Modules.Products.Queries;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Wolverine;

namespace ERP.Products.QueryApi.EndPoints;

public class GetProductsEndpoint(IMessageBus bus) : Endpoint<GetProductRequest, IResult>
{
    private readonly IMessageBus _bus = bus;

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

    public override async Task<IResult> ExecuteAsync(GetProductRequest req,
        CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<AppResult<IEnumerable<ProductResult>>>(new GetProductQuery());

        return result.ToApiResponse();
    }
}

public sealed record GetProductRequest;