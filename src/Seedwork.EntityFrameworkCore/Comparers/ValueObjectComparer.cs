using Microsoft.EntityFrameworkCore.ChangeTracking;
using Seedwork.Domain;

namespace Seedwork.EntityFrameworkCore.Comparers;

/// <summary>
/// Value comparer for <see cref="ValueObject"/> subtypes, using structural equality and <see cref="ValueObject.GetCopy"/>.
/// </summary>
public class ValueObjectComparer<T> : ValueComparer<T> where T : ValueObject
{
    public ValueObjectComparer()
        : base(
            (a, b) => a != null && b != null ? a.Equals(b) : ReferenceEquals(a, b),
            obj => obj.GetHashCode(),
            obj => (T)obj.GetCopy())
    {
    }
}
