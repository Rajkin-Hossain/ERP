using ERP.Products.Application.Commands;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Application.Result;
using MediatR;

namespace ERP.Products.Application.CommandHandlers;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, AppResult<ProductId>>
{
    private readonly IRepository<Product, ProductId> _repo;

    public UpdateProductCommandHandler(IRepository<Product, ProductId> repo)
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

        // Just update. Logic is inside Domain. If logic fails, Exception thrown.
        product.UpdateDetails(request.Name, request.ImageUrl, request.Price);

        await _repo.UpdateAsync(product, ct);

        return AppResult<ProductId>.Ok(product.Id);
    }
}
