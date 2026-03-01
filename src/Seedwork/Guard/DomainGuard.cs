using System.Text.RegularExpressions;
using Seedwork.Exceptions;

namespace Seedwork.Guard;

/// <summary>
/// Static guard clause methods for domain validation.
/// All methods return the validated value for fluent chaining.
/// All methods throw <see cref="DomainValidationException"/> on failure.
/// </summary>
public static partial class DomainGuard
{
    /// <summary>
    /// Throws if <paramref name="value"/> is null.
    /// </summary>
    public static T AgainstNull<T>(T? value, string parameterName) where T : class
    {
        return value ?? throw new DomainValidationException(
            $"{parameterName} cannot be null.", parameterName);
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is null, empty, or whitespace.
    /// </summary>
    public static string AgainstNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(
                $"{parameterName} cannot be null or empty.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is <see cref="Guid.Empty"/>.
    /// </summary>
    public static Guid AgainstEmpty(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new DomainValidationException(
                $"{parameterName} cannot be empty.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is less than <paramref name="minimum"/>.
    /// </summary>
    public static decimal AgainstLessThan(decimal value, decimal minimum, string parameterName)
    {
        if (value < minimum)
        {
            throw new DomainValidationException(
                $"{parameterName} cannot be less than {minimum}.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is outside the range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    public static int AgainstOutOfRange(int value, int min, int max, string parameterName)
    {
        if (value < min || value > max)
        {
            throw new DomainValidationException(
                $"{parameterName} must be between {min} and {max}.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is negative.
    /// </summary>
    public static decimal AgainstNegative(decimal value, string parameterName)
    {
        if (value < 0)
        {
            throw new DomainValidationException(
                $"{parameterName} cannot be negative.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is negative or zero.
    /// </summary>
    public static decimal AgainstNegativeOrZero(decimal value, string parameterName)
    {
        if (value <= 0)
        {
            throw new DomainValidationException(
                $"{parameterName} must be greater than zero.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not match the specified regex <paramref name="pattern"/>.
    /// </summary>
    public static string AgainstInvalidFormat(string value, string pattern, string parameterName)
    {
        AgainstNullOrWhiteSpace(value, parameterName);

        if (!Regex.IsMatch(value, pattern))
        {
            throw new DomainValidationException(
                $"{parameterName} has an invalid format.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if the length of <paramref name="value"/> exceeds <paramref name="maxLength"/>.
    /// </summary>
    public static string AgainstLength(string value, int maxLength, string parameterName)
    {
        AgainstNullOrWhiteSpace(value, parameterName);

        if (value.Length > maxLength)
        {
            throw new DomainValidationException(
                $"{parameterName} must not exceed {maxLength} characters.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if the length of <paramref name="value"/> is outside [<paramref name="minLength"/>, <paramref name="maxLength"/>].
    /// </summary>
    public static string AgainstLengthOutOfRange(string value, int minLength, int maxLength, string parameterName)
    {
        AgainstNullOrWhiteSpace(value, parameterName);

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new DomainValidationException(
                $"{parameterName} must be between {minLength} and {maxLength} characters.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains no elements.
    /// </summary>
    public static IEnumerable<T> AgainstEmptyCollection<T>(IEnumerable<T> value, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!value.Any())
        {
            throw new DomainValidationException(
                $"{parameterName} cannot be empty.", parameterName);
        }

        return value;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid email address.
    /// </summary>
    public static string AgainstInvalidEmail(string value, string parameterName)
    {
        AgainstNullOrWhiteSpace(value, parameterName);

        if (!EmailRegex().IsMatch(value))
        {
            throw new DomainValidationException(
                $"{parameterName} is not a valid email address.", parameterName);
        }

        return value;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
