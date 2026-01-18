using ERP.Orchestrator.Contract.Products.MessageEvents;
using MassTransit;

namespace ERP.Orchestrator.EventHandlers;

public class ProductCreatedEventHandler : IConsumer<ProductCreatedMessageEvent>
{
    private readonly ProductManagementOrchestrator orchestrator;

    public ProductCreatedEventHandler(ProductManagementOrchestrator orchestrator)
    {
        this.orchestrator = orchestrator;
    }

    public async Task Consume(ConsumeContext<ProductCreatedMessageEvent> context)
    {
        await orchestrator.ProductCreatedEventHandler(context.Message);
    }
}

