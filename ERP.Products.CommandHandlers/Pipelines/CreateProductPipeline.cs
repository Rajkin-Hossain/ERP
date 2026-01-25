using ERP.Products.CommandHandlers.Interfaces;
using ERP.Shared.Application.Interfaces;
using ERP.Shared.Command.Contracts.Modules.Products;

namespace ERP.Products.CommandHandlers.Pipelines;

public sealed class CreateProductPipeline(IDispatcherJobScheduler dispatchTrigger)
{
    public Task After(CreateProductCommand cmd, CancellationToken ct)
    {
        dispatchTrigger.EnqueueJob<IProductOutboxDispatcher>();

        return Task.CompletedTask;
    }
}
