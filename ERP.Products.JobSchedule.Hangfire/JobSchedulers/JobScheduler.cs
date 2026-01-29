using ERP.Shared.Application.Abstractions.Interfaces;
using Hangfire;

namespace ERP.Products.JobSchedule.Hangfire.JobSchedulers;

public class JobScheduler : IJobScheduler
{
    public void EnqueueJob<T>() where T : IJob
    {
        BackgroundJob.Enqueue<T>(job => job.ExecuteAsync());
    }
}






