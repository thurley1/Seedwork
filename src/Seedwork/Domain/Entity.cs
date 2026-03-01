namespace Seedwork.Domain;

/// <summary>
/// Base class for domain entities with identity-based equality.
/// </summary>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    /// <summary>
    /// The entity's unique identifier.
    /// </summary>
    public TId Id { get; protected init; }

    /// <summary>
    /// Creates a new entity with the specified identifier.
    /// </summary>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Parameterless constructor for ORM hydration.
    /// </summary>
    protected Entity()
    {
        Id = default!;
    }

    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        => !Equals(left, right);

    public override string ToString() => $"{GetType().Name} [{Id}]";
}
