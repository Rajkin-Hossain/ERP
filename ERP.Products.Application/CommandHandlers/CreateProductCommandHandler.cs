using ERP.Products.Application.Commands;
using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Result;
using MediatR;

namespace ERP.Products.Application.CommandHandlers;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, AppResult<ProductId>>
{
    private readonly IProductWriteRepository _repo;

    public CreateProductCommandHandler(IProductWriteRepository repo)
    {
        _repo = repo;
    }

    public async Task<AppResult<ProductId>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = Product.Create(
            request.Name,
            Guid.Parse(request.CategoryId),
            request.ImageUrl,
            request.Price);

        await _repo.InsertAsync(product, ct);

        return AppResult<ProductId>.Ok(product.Id);
    }
}
