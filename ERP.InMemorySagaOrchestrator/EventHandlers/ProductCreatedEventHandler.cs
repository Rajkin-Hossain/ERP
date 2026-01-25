using ERP.Shared.Event.Contracts.Modules.Products;

namespace ERP.InMemorySagaOrchestrator.EventHandlers;

public class ProductCreatedEventHandler(Saga orchestrator)
{
    private readonly Saga orchestrator = orchestrator;

    public async Task Handle(ProductCreatedEvent @event)
    {
        await orchestrator.ProductCreatedEventHandler(@event);
    }
}


