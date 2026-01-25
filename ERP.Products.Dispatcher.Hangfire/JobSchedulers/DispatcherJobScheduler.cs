using ERP.Shared.Application.Abstractions.Interfaces;
using Hangfire;

namespace ERP.Products.Dispatcher.Hangfire.JobSchedulers;

public class DispatcherJobScheduler : IDispatcherJobScheduler
{
    public void EnqueueJob<T>() where T : IDispatcher
    {
        BackgroundJob.Enqueue<T>(job => job.ExecuteAsync());
    }
}






