namespace Seedwork.Abstractions;

/// <summary>
/// Generic repository interface for aggregate roots.
/// </summary>
public interface IRepository<TAggregate, in TId>
    where TAggregate : class, IAggregateRoot
{
    /// <summary>
    /// The unit of work backing this repository.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Finds an aggregate by its identifier, or null if not found.
    /// </summary>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new aggregate to the repository.
    /// </summary>
    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing aggregate in the repository.
    /// </summary>
    Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an aggregate by its identifier.
    /// </summary>
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}
