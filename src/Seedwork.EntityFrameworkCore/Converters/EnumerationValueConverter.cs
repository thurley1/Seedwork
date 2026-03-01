using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Seedwork.Domain;

namespace Seedwork.EntityFrameworkCore.Converters;

/// <summary>
/// Converts an <see cref="Enumeration{TEnum}"/> to and from <c>int</c> for EF Core storage.
/// </summary>
public class EnumerationValueConverter<TEnum> : ValueConverter<TEnum, int>
    where TEnum : Enumeration<TEnum>
{
    public EnumerationValueConverter()
        : base(
            e => e.Value,
            value => Enumeration<TEnum>.FromValue(value))
    {
    }
}
