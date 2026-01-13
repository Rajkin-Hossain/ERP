using ERP.SharedKernal.Interfaces;
using System.Collections.ObjectModel;

namespace ERP.SharedKernal.Entities;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => new ReadOnlyCollection<IDomainEvent>(_domainEvents);

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
