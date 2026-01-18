namespace ERP.Products.Application.Interfaces;

public interface IOutboxDispatchTrigger
{
    void EnqueueJob();
}



