using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Seedwork.Domain;

namespace Seedwork.EntityFrameworkCore.Converters;

/// <summary>
/// Automatically selects <see cref="IdValueConverter{TId}"/> for all <see cref="Id{T}"/> subtypes.
/// </summary>
public class IdValueConverterSelector : ValueConverterSelector
{
    public IdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
        : base(dependencies)
    {
    }

    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
    {
        var baseConverters = base.Select(modelClrType, providerClrType);

        foreach (var converter in baseConverters)
        {
            yield return converter;
        }

        if (IsIdType(modelClrType))
        {
            var converterType = typeof(IdValueConverter<>).MakeGenericType(modelClrType);

            yield return new ValueConverterInfo(
                modelClrType,
                typeof(Guid),
                info => (ValueConverter)Activator.CreateInstance(converterType)!);
        }
    }

    private static bool IsIdType(Type type)
    {
        while (type is not null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Id<>))
            {
                return true;
            }

            type = type.BaseType!;
        }

        return false;
    }
}
