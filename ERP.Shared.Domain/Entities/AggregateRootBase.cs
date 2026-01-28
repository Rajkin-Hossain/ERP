using ERP.Shared.Domain.Interfaces;

namespace ERP.Shared.Domain.Entities;

public abstract class AggregateRootBase
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events;

    protected void Raise(IDomainEvent @event) => _events.Add(@event);

    public void ClearDomainEvents() => _events.Clear();

    public abstract string GetAggregateId();
}
