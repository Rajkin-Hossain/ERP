using ERP.Products.Application.Commands;
using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Result;
using MediatR;
namespace ERP.Products.Application.CommandHandlers;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, AppResult<ProductId>>
{
    private readonly IProductWriteRepository _repo;

    public UpdateProductCommandHandler(IProductWriteRepository repo)
    {
        _repo = repo;
    }

    public async Task<AppResult<ProductId>> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _repo.FindAsync(request.ProductId, ct);

        if (product == null)
        {
            return AppResult<ProductId>.Fail(new AppError(AppErrorType.NotFound, $"Product with Id {request.ProductId} was not found."));
        }

        // Validate Domain Value Objects
        var nameResult = ProductName.Create(request.Name);
        var imageResult = ImageUrl.Create(request.ImageUrl);
        var priceResult = Price.Create(request.Price);

        // Check for any domain-level failures
        if (!nameResult.IsSuccess) return AppResult<ProductId>.Fail(new AppError(AppErrorType.Validation, nameResult.ErrorMessage!));
        if (!imageResult.IsSuccess) return AppResult<ProductId>.Fail(new AppError(AppErrorType.Validation, imageResult.ErrorMessage!));
        if (!priceResult.IsSuccess) return AppResult<ProductId>.Fail(new AppError(AppErrorType.Validation, priceResult.ErrorMessage!));

        product.UpdateDetails(nameResult.Value!, imageResult.Value!, priceResult.Value!);

        await _repo.UpdateAsync(product, ct);

        return AppResult<ProductId>.Ok(product.Id);
    }
}







