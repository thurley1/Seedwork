using System.Reflection;

namespace Seedwork.Domain;

/// <summary>
/// Smart enum base class with a finite set of named instances.
/// </summary>
/// <typeparam name="TEnum">The concrete enumeration type (CRTP).</typeparam>
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>, IComparable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Lazy<IReadOnlyCollection<TEnum>> AllItems = new(GetAllItems);

    /// <summary>
    /// The numeric value of this enumeration member.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// The display name of this enumeration member.
    /// </summary>
    public string Name { get; }

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    /// <summary>
    /// Returns all declared members of this enumeration.
    /// </summary>
    public static IReadOnlyCollection<TEnum> GetAll() => AllItems.Value;

    /// <summary>
    /// Finds a member by its numeric value. Throws if not found.
    /// </summary>
    public static TEnum FromValue(int value) =>
        GetAll().FirstOrDefault(e => e.Value == value)
        ?? throw new InvalidOperationException($"'{value}' is not a valid value for {typeof(TEnum).Name}.");

    /// <summary>
    /// Finds a member by its name (case-insensitive). Throws if not found.
    /// </summary>
    public static TEnum FromName(string name) =>
        GetAll().FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"'{name}' is not a valid name for {typeof(TEnum).Name}.");

    /// <summary>
    /// Attempts to find a member by its numeric value.
    /// </summary>
    public static bool TryFromValue(int value, out TEnum? result)
    {
        result = GetAll().FirstOrDefault(e => e.Value == value);
        return result is not null;
    }

    /// <summary>
    /// Attempts to find a member by its name (case-insensitive).
    /// </summary>
    public static bool TryFromName(string name, out TEnum? result)
    {
        result = GetAll().FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));
        return result is not null;
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null) return false;
        return GetType() == other.GetType() && Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as Enumeration<TEnum>);

    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(Enumeration<TEnum>? other) => other is null ? 1 : Value.CompareTo(other.Value);

    public static bool operator ==(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
        => Equals(left, right);

    public static bool operator !=(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
        => !Equals(left, right);

    public override string ToString() => Name;

    private static IReadOnlyCollection<TEnum> GetAllItems()
    {
        return typeof(TEnum)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(TEnum))
            .Select(f => (TEnum)f.GetValue(null)!)
            .ToList()
            .AsReadOnly();
    }
}
