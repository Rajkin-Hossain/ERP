using ERP.Shared.Application.Interfaces;
using Hangfire;

namespace ERP.Products.JobSchedule.Hangfire.JobSchedulers;

public class JobScheduler : IJobScheduler
{
    public void EnqueueJob<T>() where T : IJob
    {
        BackgroundJob.Enqueue<T>(job => job.ExecuteAsync());
    }
}






