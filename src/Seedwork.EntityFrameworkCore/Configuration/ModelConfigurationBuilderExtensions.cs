using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Seedwork.Domain;
using Seedwork.EntityFrameworkCore.Converters;

namespace Seedwork.EntityFrameworkCore.Configuration;

/// <summary>
/// Extension methods for <see cref="ModelConfigurationBuilder"/> to register Seedwork conventions.
/// </summary>
public static class ModelConfigurationBuilderExtensions
{
    /// <summary>
    /// Registers the <see cref="IdValueConverterSelector"/> so all <see cref="Id{T}"/> subtypes
    /// are automatically converted to <see cref="Guid"/>.
    /// </summary>
    public static ModelConfigurationBuilder ConfigureIdConventions(this ModelConfigurationBuilder builder)
    {
        builder.Conventions.Add(
            _ => new IdValueConverterConvention());

        return builder;
    }

    /// <summary>
    /// Registers the <see cref="EnumerationValueConverter{TEnum}"/> for a specific enumeration type.
    /// </summary>
    public static ModelConfigurationBuilder ConfigureEnumerationConventions<TEnum>(this ModelConfigurationBuilder builder)
        where TEnum : Enumeration<TEnum>
    {
        builder.Properties<TEnum>()
            .HaveConversion<EnumerationValueConverter<TEnum>>();

        return builder;
    }
}

/// <summary>
/// Convention that configures all Id&lt;T&gt; properties to use IdValueConverter.
/// </summary>
internal class IdValueConverterConvention : Microsoft.EntityFrameworkCore.Metadata.Conventions.IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
        Microsoft.EntityFrameworkCore.Metadata.Builders.IConventionModelBuilder modelBuilder,
        Microsoft.EntityFrameworkCore.Metadata.Conventions.IConventionContext<Microsoft.EntityFrameworkCore.Metadata.Builders.IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (IsIdType(property.ClrType))
                {
                    var converterType = typeof(IdValueConverter<>).MakeGenericType(property.ClrType);
                    var converter = (ValueConverter)Activator.CreateInstance(converterType)!;
                    property.SetValueConverter(converter);
                }
            }
        }
    }

    private static bool IsIdType(Type type)
    {
        var current = type;
        while (current is not null)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(Id<>))
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }
}
