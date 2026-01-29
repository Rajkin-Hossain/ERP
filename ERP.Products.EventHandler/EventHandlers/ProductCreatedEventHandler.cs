using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Abstractions.Interfaces;
using ERP.Shared.Event.Contracts.Modules.Products;
using ERP.Shared.Kernel.Result;

namespace ERP.Products.EventHandler.EventHandlers;

public sealed class ProductCreatedEventHandler(IReadRepository<Product, ProductId> repo)
{
    private readonly IReadRepository<Product, ProductId> _repo = repo;

    public async Task<AppResult<Guid>> Handle(ProductCreatedEvent @event, CancellationToken ct)
    {
        //TODO: Update to Read Database

        return AppResult<Guid>.Ok(Guid.NewGuid());
    }
}