namespace Seedwork.Exceptions;

/// <summary>
/// Thrown when an entity cannot be found by its identifier.
/// </summary>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// The type of entity that was not found.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    /// The identifier that was searched for.
    /// </summary>
    public object? EntityId { get; }

    public EntityNotFoundException(Type entityType, object? entityId)
        : base($"{entityType.Name} with id '{entityId}' was not found.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    /// <summary>
    /// Creates an <see cref="EntityNotFoundException"/> for the specified entity type.
    /// </summary>
    public static EntityNotFoundException For<TEntity>(object? id) =>
        new(typeof(TEntity), id);
}
