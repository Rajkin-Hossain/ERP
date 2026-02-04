namespace ERP.Shared.Application.Interfaces;

public interface IJobScheduler
{
    void EnqueueJob<T>() where T : IJob;
}



