using BuildingBlocks.Application.Result;
using ERP.Products.Application.Commands;
using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.SharedKernal.Interfaces;
using MediatR;

namespace ERP.Products.Application.CommandHandlers;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, AppResult<ProductId>>
{
    private readonly IProductWriteRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductWriteRepository repo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppResult<ProductId>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = Product.Create(request.Name, Guid.Parse(request.CategoryId), request.ImageUrl, request.Price);

        var productId = await _unitOfWork.StartTransactionAsync(async ct =>
        {
            await _repo.InsertAsync(product, ct);

            return product.Id;
        }, ct);

        return AppResult<ProductId>.Ok(productId);
    }
}
