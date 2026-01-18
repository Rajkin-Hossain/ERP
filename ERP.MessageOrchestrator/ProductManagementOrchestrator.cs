using ERP.MessageOrchestrator.Contracts.Products.MessageCommands;
using ERP.MessageOrchestrator.Contracts.Products.MessageEvents;
using ERP.MessageOrchestrator.Interfaces;

namespace ERP.MessageOrchestrator;

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


