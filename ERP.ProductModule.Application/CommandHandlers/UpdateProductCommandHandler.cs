using ERP.ProductModule.Application.Commands;
using ERP.ProductModule.Application.RepoInterfaces;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.AppResult;
using ERP.SharedKernal.Interfaces;
using MediatR;

namespace ERP.ProductModule.Application.CommandHandlers;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, AppResult<ProductId>>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppResult<ProductId>> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _repo.FindAsync(request.ProductId, ct);

        if (product == null)
        {
            return AppResult<ProductId>.Fail(new AppError(ErrorType.NotFound, $"Product with Id {request.ProductId} was not found."));
        }

        product.UpdateDetails(request.Name, request.ImageUrl, request.Price);

        var productId = await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            _repo.Update(product);

            await _unitOfWork.SaveChangesAsync(ct);

            return product.Id;
        }, ct);

        return AppResult<ProductId>.Ok(productId);
    }
}
