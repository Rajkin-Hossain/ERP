namespace ERP.Shared.Application.Abstractions.Interfaces;

public sealed record UnitOfWorkExecutionOptions(
    bool EnableOutbox = true,
    bool UseChangeTracker = true)
{
    public bool UseOutbox => EnableOutbox && UseChangeTracker;

    public static UnitOfWorkExecutionOptions Default { get; } = new();
    public static UnitOfWorkExecutionOptions WithoutOutbox { get; } = new(false, true);
    public static UnitOfWorkExecutionOptions WithoutChangeTracking { get; } = new(false, false);
}
