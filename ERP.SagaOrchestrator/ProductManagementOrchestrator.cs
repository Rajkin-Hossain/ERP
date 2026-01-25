using ERP.Contracts.MessageOrchestrator.Products.MessageCommands;
using ERP.Contracts.SagaOrchestrator.Products.Events;
using ERP.SagaOrchestrator.Interfaces;

namespace ERP.SagaOrchestrator;

public class ProductManagementOrchestrator
{
    private readonly IPublisherBus _serviceBus;

    public ProductManagementOrchestrator(IPublisherBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    public async Task ProductCreatedEventHandler(ProductCreatedEvent e)
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


