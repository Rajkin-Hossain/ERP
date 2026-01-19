using ERP.Contracts.MessageOrchestrator.Products.MessageEvents;
using MassTransit;

namespace ERP.MessageOrchestrator.EventHandlers;

public class ProductPriceUpdatedEventHandler : IConsumer<ProductPriceUpdatedMessageEvent>
{
    private readonly ProductManagementOrchestrator orchestrator;

    public ProductPriceUpdatedEventHandler(ProductManagementOrchestrator orchestrator)
    {
        this.orchestrator = orchestrator;
    }

    public async Task Consume(ConsumeContext<ProductPriceUpdatedMessageEvent> context)
    {
        await orchestrator.ProductPriceUpdatedEventHandler(context.Message);
    }
}
