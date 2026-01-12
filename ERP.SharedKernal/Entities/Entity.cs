namespace ERP.SharedKernal.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; }

    public bool IsTransient => Id == Guid.Empty;

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;

        //Reference equality checker very faster
        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (IsTransient || other.IsTransient)
            return false;

        return Id == other.Id;
    }

    public override bool Equals(object? obj)
        => Equals(obj as Entity);

    public override int GetHashCode()
    {
        if (IsTransient)
            return base.GetHashCode();

        return HashCode.Combine(GetType(), Id);
    }

    public static bool operator ==(Entity? left, Entity? right)
        => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right)
        => !Equals(left, right);
}

