using Microsoft.EntityFrameworkCore;
using Seedwork.Abstractions;

namespace Seedwork.EntityFrameworkCore.Configuration;

/// <summary>
/// Extension methods for <see cref="ModelBuilder"/>.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Ignores the <see cref="IAggregateRoot.DomainEvents"/> property on all entity types
    /// that implement <see cref="IAggregateRoot"/>.
    /// </summary>
    public static ModelBuilder IgnoreDomainEvents(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAggregateRoot).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Ignore(nameof(IAggregateRoot.DomainEvents));
            }
        }

        return modelBuilder;
    }
}
