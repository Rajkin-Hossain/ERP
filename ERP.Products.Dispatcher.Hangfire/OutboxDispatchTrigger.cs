using ERP.Products.Application.Interfaces;
using Hangfire;

namespace ERP.Products.Dispatcher.Hangfire;

public class OutboxDispatchTrigger : IOutboxDispatchTrigger
{
    public void EnqueueJob()
    {
        BackgroundJob.Enqueue<IProductOutboxDispatcher>(job => job.ExecuteAsync());
    }
}
