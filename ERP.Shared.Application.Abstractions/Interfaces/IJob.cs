namespace ERP.Shared.Application.Abstractions.Interfaces;

public interface IJob
{
    Task ExecuteAsync(CancellationToken ct = default);
}



