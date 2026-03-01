using Seedwork.Guard;

namespace Seedwork.Domain;

/// <summary>
/// Strongly-typed identifier wrapping a <see cref="Guid"/>.
/// </summary>
/// <typeparam name="T">The concrete Id type (CRTP).</typeparam>
public abstract record Id<T>(Guid Value) where T : Id<T>
{
    /// <summary>
    /// Creates a new identifier with a random Guid.
    /// </summary>
    public static T New() => Create(Guid.NewGuid());

    /// <summary>
    /// Creates an identifier from an existing Guid. Throws if empty.
    /// </summary>
    public static T From(Guid value)
    {
        DomainGuard.AgainstEmpty(value, nameof(value));
        return Create(value);
    }

    /// <summary>
    /// Parses a string representation of a Guid into a typed identifier.
    /// </summary>
    public static T Parse(string value) => From(Guid.Parse(value));

    /// <summary>
    /// Implicitly converts to the underlying Guid.
    /// </summary>
    public static implicit operator Guid(Id<T> id) => id.Value;

    /// <summary>
    /// Explicitly converts a Guid to the typed identifier.
    /// </summary>
    public static explicit operator Id<T>(Guid value) => From(value);

    public sealed override string ToString() => Value.ToString();

    /// <summary>
    /// Suppresses the record's default PrintMembers behavior.
    /// </summary>
    protected virtual bool PrintMembers(System.IO.TextWriter writer)
    {
        writer.Write(Value);
        return true;
    }

    private static T Create(Guid value)
    {
        return (T)Activator.CreateInstance(typeof(T), value)!;
    }
}
