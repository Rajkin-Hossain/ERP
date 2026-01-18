using BuildingBlocks.Orchestrator.Contracts.Products.MessageEvents;
using MassTransit;

namespace ERP.Orchestrator.EventHandlers;

public class ProductCreatedEventHandler : IConsumer<ProductCreatedEvent>
{
    private readonly ProductManagementOrchestrator orchestrator;

    public ProductCreatedEventHandler(ProductManagementOrchestrator orchestrator)
    {
        this.orchestrator = orchestrator;
    }

    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        await orchestrator.ProductCreatedEventHandler(context.Message);
    }
}
