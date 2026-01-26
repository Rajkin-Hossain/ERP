using ERP.Shared.Application.Abstractions.Interfaces;

namespace ERP.Products.Dispatcher.BackgroundWorker;

public sealed class OutboxBackgroundWorker : BackgroundService
{
    private readonly ILogger<OutboxBackgroundWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(300);

    public OutboxBackgroundWorker(ILogger<OutboxBackgroundWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Background Worker is starting.");

        using var timer = new PeriodicTimer(_interval);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DispatchOutboxMessages(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Outbox Background Worker is stopping.");
        }
    }

    private async Task DispatchOutboxMessages(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Triggering Outbox Dispatcher...");

            using var scope = _scopeFactory.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();

            await dispatcher.ExecuteAsync(stoppingToken);

            _logger.LogInformation("Outbox Dispatcher completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while executing Outbox Dispatcher.");
        }
    }
}
