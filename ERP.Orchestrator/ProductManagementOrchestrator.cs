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

    public async Task ProductCreatedEventHandler(ProductCreatedMessageEvent e)
    {
        await _serviceBus.PublishAsync(
            new CreateProductMessageCommand(e.ProductId), CancellationToken.None
        );
    }

    public async Task ProductUpdatedEventHandler(ProductUpdatedMessageEvent e)
    {
        await _serviceBus.PublishAsync(
            new UpdateProductMessageCommand(e.ProductId), CancellationToken.None
        );
    }
}
