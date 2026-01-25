using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Result;
using ERP.Shared.Command.Contracts.Modules.Products;
using ERP.Shared.Presentation.Extensions;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Wolverine;

namespace ERP.Products.CommandApi.EndPoints;

public sealed class CreateProductEndpoint(IMessageBus bus) : Endpoint<CreateProductRequest, IResult>
{
    private readonly IMessageBus _bus = bus;

    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();

        // OpenAPI Metadata
        Description(x => x
            .WithSummary("Create Product")
            .WithDescription("Creates a product")
            .WithName("CreateProduct")
            .ProducesStandardApiResponses()
        );
    }

    public override async Task<IResult> ExecuteAsync(CreateProductRequest req,
        CancellationToken ct)
    {
        var command = new CreateProductCommand(
            req.Name,
            req.Price,
            req.ImageUrl,
            req.CategoryId);

        var result = await _bus.InvokeAsync<AppResult<ProductId>>(command);

        return result.ToApiResponse();
    }
}

public sealed record CreateProductRequest(
    string Name,
    decimal Price,
    string ImageUrl,
    string CategoryId);