using ERP.Contracts.SagaOrchestrator.Products.Events;
using MassTransit;

namespace ERP.SagaOrchestrator.EventHandlers;

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


