namespace ERP.Shared.Application.Abstractions.Interfaces;

public interface IJobScheduler
{
    void EnqueueJob<T>() where T : IJob;
}



