using Microsoft.EntityFrameworkCore.ChangeTracking;
using Seedwork.Domain;

namespace Seedwork.EntityFrameworkCore.Comparers;

/// <summary>
/// Value comparer for <see cref="Id{T}"/> types, comparing by underlying Guid.
/// </summary>
public class IdValueComparer<TId> : ValueComparer<TId> where TId : Id<TId>
{
    public IdValueComparer()
        : base(
            (a, b) => a != null && b != null && a.Value == b.Value,
            id => id.Value.GetHashCode(),
            id => Id<TId>.From(id.Value))
    {
    }
}
