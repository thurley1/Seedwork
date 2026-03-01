using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seedwork.Domain;
using Seedwork.EntityFrameworkCore.Comparers;
using Seedwork.EntityFrameworkCore.Converters;

namespace Seedwork.EntityFrameworkCore.Configuration;

/// <summary>
/// Extension methods for configuring Seedwork types in EF Core entity type configurations.
/// </summary>
public static class EntityTypeConfigurationExtensions
{
    /// <summary>
    /// Configures a property of type <typeparamref name="TId"/> with the appropriate converter and comparer.
    /// </summary>
    public static PropertyBuilder<TId> ConfigureId<TId>(this PropertyBuilder<TId> builder)
        where TId : Id<TId>
    {
        builder.HasConversion(new IdValueConverter<TId>());
        builder.Metadata.SetValueComparer(new IdValueComparer<TId>());
        return builder;
    }

    /// <summary>
    /// Configures a property of type <typeparamref name="TEnum"/> with the enumeration value converter.
    /// </summary>
    public static PropertyBuilder<TEnum> ConfigureEnumeration<TEnum>(this PropertyBuilder<TEnum> builder)
        where TEnum : Enumeration<TEnum>
    {
        return builder
            .HasConversion(new EnumerationValueConverter<TEnum>());
    }
}
