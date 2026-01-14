using ERP.Orchestrator.Contract.ProductModule.Events;
using MassTransit;

namespace ERP.Orchestrator.EventHandlers;

public class ProductUpdatedEventHandler : IConsumer<ProductUpdatedEvent>
{
    private readonly ProductManagementOrchestrator orchestrator;

    public ProductUpdatedEventHandler(ProductManagementOrchestrator orchestrator)
    {
        this.orchestrator = orchestrator;
    }

    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        await orchestrator.ProductUpdatedEventHandler(context.Message);
    }
}
