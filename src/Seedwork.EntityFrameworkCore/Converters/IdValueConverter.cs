using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Seedwork.Domain;

namespace Seedwork.EntityFrameworkCore.Converters;

/// <summary>
/// Converts an <see cref="Id{T}"/> to and from <see cref="Guid"/> for EF Core storage.
/// </summary>
public class IdValueConverter<TId> : ValueConverter<TId, Guid> where TId : Id<TId>
{
    public IdValueConverter()
        : base(
            id => id.Value,
            guid => Id<TId>.From(guid))
    {
    }
}
