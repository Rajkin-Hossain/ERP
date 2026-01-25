namespace ERP.Shared.Application.Abstractions.Interfaces;

public interface IDispatcher
{
    Task ExecuteAsync(CancellationToken ct = default);
}



