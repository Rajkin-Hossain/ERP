using ERP.Contracts.SagaOrchestrator.Products.MessageEvents;
using ERP.SagaOrchestrator;
using MassTransit;

namespace ERP.SagaOrchestrator.EventHandlers;

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


