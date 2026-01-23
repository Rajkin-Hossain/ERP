using ERP.Products.Application.Commands;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Application.Result;
using MediatR;

namespace ERP.Products.Application.CommandHandlers;

public sealed class UpdateProductPriceCommandHandler(
    IRepository<Product, ProductId> repository)
    : IRequestHandler<UpdateProductPriceCommand, AppResult<ProductId>>
{
    public async Task<AppResult<ProductId>> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.FindAsync(request.ProductId, cancellationToken);

        if (product == null)
        {
            return AppResult<ProductId>.Fail(new AppError(AppErrorType.NotFound, "Product not found"));
        }

        product.UpdatePrice(Price.Create(request.NewPrice));

        await repository.UpdateAsync(product, cancellationToken);

        return AppResult<ProductId>.Ok(product.Id);
    }
}
