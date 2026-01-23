using ERP.Contracts.MessageOrchestrator.Products.MessageCommands;
using ERP.Contracts.MessageOrchestrator.Products.MessageEvents;
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
        await _serviceBus.PublishCommandAsync(
            new CreateProductMessageCommand(
                e.ProductId,
                e.Name,
                e.CategoryId,
                e.ImageUrl,
                e.Price),
            CancellationToken.None
        );
    }

    public async Task ProductUpdatedEventHandler(ProductUpdatedMessageEvent e)
    {
        await _serviceBus.PublishCommandAsync(
            new UpdateProductMessageCommand(
                e.ProductId,
                e.Name,
                e.CategoryId,
                e.ImageUrl,
                e.Price),
            CancellationToken.None
        );
    }

    public async Task ProductPriceUpdatedEventHandler(ProductPriceUpdatedMessageEvent e)
    {
        await _serviceBus.PublishCommandAsync(
            new UpdateProductPriceMessageCommand(
                e.ProductId,
                e.NewPrice),
            CancellationToken.None
        );
    }
}


