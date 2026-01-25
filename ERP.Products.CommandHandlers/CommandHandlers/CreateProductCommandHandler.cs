using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Application.Result;
using ERP.Shared.Command.Contracts.Modules.Products;

namespace ERP.Products.CommandHandlers.CommandHandlers;

public sealed class CreateProductCommandHandler(IRepository<Product, ProductId> repo, IUnitOfWork unitOfWork)
{
    private readonly IRepository<Product, ProductId> _repo = repo;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AppResult<ProductId>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = Product.Create(
            request.Name,
            request.CategoryId,
            request.ImageUrl,
            request.Price);

        var productId = await _unitOfWork.ExecuteAsync(async ct =>
        {
            await _repo.InsertAsync(product, ct);

            return product.Id;
        }, ct);


        return AppResult<ProductId>.Ok(product.Id);
    }
}