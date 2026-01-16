using ERP.ProductModule.Application.Commands;
using ERP.ProductModule.Application.RepoInterfaces.Write;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.AppResult;
using ERP.SharedKernal.Interfaces;
using MediatR;

namespace ERP.ProductModule.Application.CommandHandlers;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, AppResult<ProductId>>
{
    private readonly IProductWriteRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductWriteRepository repo, 
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppResult<ProductId>> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _repo.FindAsync(Guid.Parse(request.ProductId), ct);

        if (product == null)
        {
            return AppResult<ProductId>.Fail(new AppError(ErrorType.NotFound, $"Product with Id {request.ProductId} was not found."));
        }

        product.UpdateDetails(request.Name, request.ImageUrl, request.Price);

        var productId = await _unitOfWork.StartTransactionAsync(async ct =>
        {
            await _repo.UpdateAsync(product, ct);

            return product.Id;
        }, ct);

        return AppResult<ProductId>.Ok(productId);
    }
}
