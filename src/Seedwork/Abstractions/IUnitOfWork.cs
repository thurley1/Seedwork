namespace Seedwork.Abstractions;

/// <summary>
/// Represents a unit of work for persisting domain changes.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all pending changes and returns the number of state entries written.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
