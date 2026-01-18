using ERP.MessageOrchestrator;
using ERP.MessageOrchestrator.Contract.Products.MessageEvents;
using MassTransit;

namespace ERP.MessageOrchestrator.EventHandlers;

public class ProductUpdatedEventHandler : IConsumer<ProductUpdatedMessageEvent>
{
    private readonly ProductManagementOrchestrator orchestrator;

    public ProductUpdatedEventHandler(ProductManagementOrchestrator orchestrator)
    {
        this.orchestrator = orchestrator;
    }

    public async Task Consume(ConsumeContext<ProductUpdatedMessageEvent> context)
    {
        await orchestrator.ProductUpdatedEventHandler(context.Message);
    }
}

