using ERP.Products.Application.Commands;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Application.Result;
using MediatR;

namespace ERP.Products.Application.CommandHandlers;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, AppResult<ProductId>>
{
    private readonly IRepository<Product, ProductId> _repo;

    public CreateProductCommandHandler(IRepository<Product, ProductId> repo)
    {
        _repo = repo;
    }

    public async Task<AppResult<ProductId>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = Product.Create(
            request.Name,
            request.CategoryId,
            request.ImageUrl,
            request.Price);

        await _repo.InsertAsync(product, ct);

        return AppResult<ProductId>.Ok(product.Id);
    }
}
