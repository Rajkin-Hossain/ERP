using ERP.Shared.Application.Interfaces;
using ERP.Shared.Command.Contracts.Modules.Products;

namespace ERP.Products.CommandHandlers.Pipelines;

public sealed class CreateProductPipeline(IJobScheduler jobScheduler)
{
    public Task After(CreateProductCommand cmd)
    {
        /*Enqueue Job function implementation could be for background or Hangfire depends on where IJobScheduler is implemented
            Currently IJobScheduler implementation is in Hangfire so it will use Hangfire to enqueue the job*/

        /*IOutboxJob:IJob is for outbox we know that we can define any job here (:IJob)
            now as we were supposed to send the Bus, then we decided to use Outbox pattern to ensure the message
            is sent once the transaction is completed, so IOutboxJob implementation sits inside Bus Infrastructure project*/

        jobScheduler.EnqueueJob<IOutboxJob>();

        return Task.CompletedTask;
    }
}
