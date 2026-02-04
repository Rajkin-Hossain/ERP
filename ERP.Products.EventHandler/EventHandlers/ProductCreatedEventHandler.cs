using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Event.Contracts.Modules.Products;

namespace ERP.Products.EventHandler.EventHandlers;

public sealed class ProductCreatedEventHandler(IReadRepository<Product, ProductId> repo)
{
    private readonly IReadRepository<Product, ProductId> _repo = repo;

    public async Task Handle(ProductCreatedEvent @event, CancellationToken ct)
    {
        //TODO: Sync to Read Database from write database
    }
}