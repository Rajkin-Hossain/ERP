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
        // 1. Validate Value Objects
        var nameResult = ProductName.Create(request.Name);
        var imageResult = ImageUrl.Create(request.ImageUrl);
        var priceResult = Price.Create(request.Price);
        var categoryId = new CategoryId(Guid.Parse(request.CategoryId));

        // 2. Aggregate Domain Errors
        if (!nameResult.IsSuccess) return AppResult<ProductId>.Fail(new AppError(AppErrorType.Validation, nameResult.ErrorMessage!));
        if (!imageResult.IsSuccess) return AppResult<ProductId>.Fail(new AppError(AppErrorType.Validation, imageResult.ErrorMessage!));
        if (!priceResult.IsSuccess) return AppResult<ProductId>.Fail(new AppError(AppErrorType.Validation, priceResult.ErrorMessage!));

        // 3. Create Entity
        var product = Product.Create(
            nameResult.Value!,
            categoryId,
            imageResult.Value!,
            priceResult.Value!);

        await _repo.InsertAsync(product, ct);

        return AppResult<ProductId>.Ok(product.Id);
    }
}







