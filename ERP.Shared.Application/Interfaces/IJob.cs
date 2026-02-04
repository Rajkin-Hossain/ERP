namespace ERP.Shared.Application.Interfaces;

public interface IJob
{
    Task ExecuteAsync(CancellationToken ct = default);
}



