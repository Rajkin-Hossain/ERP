using ERP.Shared.Application.Interfaces;

using Hangfire;

using ERP.Products.Application.Interfaces;
namespace ERP.Products.Dispatcher.Hangfire;

public class OutboxDispatchTrigger : IOutboxDispatchTrigger
{
    public void EnqueueJob()
    {
        BackgroundJob.Enqueue<IProductOutboxDispatcher>(job => job.ExecuteAsync());
    }
}






