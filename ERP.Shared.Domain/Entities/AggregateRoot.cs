using ERP.Shared.Domain.Interfaces;

namespace ERP.Shared.Domain.Entities;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : notnull
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events;

    protected void AddDomainEvent(IDomainEvent @event) => _events.Add(@event);

    public void ClearDomainEvents() => _events.Clear();

    public string GetAggregateId() => Id.ToString()!;
}




