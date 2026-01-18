namespace ERP.Products.Application.Interfaces;

public interface IProductOutboxDispatcher
{
    Task ExecuteAsync(CancellationToken ct = default);
}



