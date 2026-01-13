using ERP.ProductModule.Application.Commands;
using ERP.ProductModule.Application.RepoInterfaces;
using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.AppResult;
using ERP.SharedKernal.Interfaces;
using MediatR;

namespace ERP.ProductModule.Application.CommandHandlers;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, AppResult<ProductId>>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppResult<ProductId>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = Product.Create(request.Name, request.CategoryId, request.ImageUrl, request.Price);

        var productId = await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            _repo.Insert(product);

            await _unitOfWork.SaveChangesAsync(ct);

            return product.Id;
        }, ct);

        return AppResult<ProductId>.Ok(productId);
    }
}
