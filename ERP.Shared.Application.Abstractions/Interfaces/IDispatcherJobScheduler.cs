namespace ERP.Shared.Application.Abstractions.Interfaces;

public interface IDispatcherJobScheduler
{
    void EnqueueJob<T>() where T : IDispatcher;
}



