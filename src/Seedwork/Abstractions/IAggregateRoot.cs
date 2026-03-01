namespace Seedwork.Abstractions;

/// <summary>
/// Marks an entity as an aggregate root, exposing domain events.
/// Used as a constraint on <see cref="IRepository{TAggregate, TId}"/>.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Domain events raised by this aggregate.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears all pending domain events.
    /// </summary>
    void ClearDomainEvents();
}
