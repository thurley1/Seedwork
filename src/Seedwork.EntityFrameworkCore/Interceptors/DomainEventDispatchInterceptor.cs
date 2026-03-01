using Microsoft.EntityFrameworkCore.Diagnostics;
using Seedwork.Abstractions;

namespace Seedwork.EntityFrameworkCore.Interceptors;

/// <summary>
/// Interceptor that dispatches domain events after <c>SaveChangesAsync</c> succeeds.
/// </summary>
public class DomainEventDispatchInterceptor : SaveChangesInterceptor
{
    private readonly Func<IDomainEvent, CancellationToken, Task> _dispatch;

    /// <summary>
    /// Creates a new interceptor with the specified dispatch function.
    /// </summary>
    /// <param name="dispatch">A function to handle each domain event.</param>
    public DomainEventDispatchInterceptor(Func<IDomainEvent, CancellationToken, Task> dispatch)
    {
        ArgumentNullException.ThrowIfNull(dispatch);
        _dispatch = dispatch;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            var aggregateRoots = eventData.Context.ChangeTracker
                .Entries<IAggregateRoot>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Count > 0)
                .ToList();

            var domainEvents = aggregateRoots
                .SelectMany(ar => ar.DomainEvents)
                .ToList();

            foreach (var aggregateRoot in aggregateRoots)
            {
                aggregateRoot.ClearDomainEvents();
            }

            foreach (var domainEvent in domainEvents)
            {
                await _dispatch(domainEvent, cancellationToken);
            }
        }

        return result;
    }
}
