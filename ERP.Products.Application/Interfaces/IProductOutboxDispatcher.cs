using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
namespace ERP.Products.Application.Interfaces;

public interface IProductOutboxDispatcher
{
    Task ExecuteAsync(CancellationToken ct = default);
}



