using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Orchestrator.Contracts.Products.MessageCommands;
using BuildingBlocks.Orchestrator.Contracts.Products.MessageEvents;

namespace ERP.Orchestrator;

public class ProductManagementOrchestrator
{
    private readonly IServiceBus _serviceBus;

    public ProductManagementOrchestrator(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    public async Task ProductCreatedEventHandler(ProductCreatedEvent e)
    {
        await _serviceBus.PublishAsync(
            new CreateProductCommand(e.ProductId), CancellationToken.None
        );
    }

    public async Task ProductUpdatedEventHandler(ProductUpdatedEvent e)
    {
        await _serviceBus.PublishAsync(
            new UpdateProductCommand(e.ProductId), CancellationToken.None
        );
    }
}
