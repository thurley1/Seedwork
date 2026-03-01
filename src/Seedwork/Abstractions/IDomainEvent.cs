namespace Seedwork.Abstractions;

/// <summary>
/// Marker interface for domain events.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// The UTC timestamp when this event occurred.
    /// </summary>
    DateTime OccurredOnUtc { get; }
}
