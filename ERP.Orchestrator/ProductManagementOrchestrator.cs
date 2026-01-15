using ERP.Orchestrator.Contract.ProductModule.Commands;
using ERP.Orchestrator.Contract.ProductModule.Events;
using ERP.SharedKernal.Interfaces;

namespace ERP.Orchestrator;

public class ProductManagementOrchestrator
{
    private readonly IServiceBus _serviceBus;

    public ProductManagementOrchestrator(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    public async Task ProductCreatedEventHandler(ProductCreatedEvent e)
    {
        await _serviceBus.PublishCommand(
            new CreateProductCommand(e.ProductId), CancellationToken.None
        );
    }

    public async Task ProductUpdatedEventHandler(ProductUpdatedEvent e)
    {
        await _serviceBus.PublishCommand(
            new UpdateProductCommand(e.ProductId), CancellationToken.None
        );
    }
}
