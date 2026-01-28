namespace ERP.Shared.Domain.Entities;

public abstract class AggregateRoot<TId> : AggregateRootBase where TId : notnull
{
    public TId Id { get; protected set; } = default!;
    public override string GetAggregateId() => Id.ToString()!;
}




