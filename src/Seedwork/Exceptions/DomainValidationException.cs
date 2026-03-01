namespace Seedwork.Exceptions;

/// <summary>
/// Thrown when a domain validation rule is violated.
/// Carries the parameter name for structured API error responses.
/// </summary>
public class DomainValidationException : DomainException
{
    /// <summary>
    /// The name of the parameter that failed validation.
    /// </summary>
    public string ParameterName { get; }

    public DomainValidationException(string message, string parameterName)
        : base(message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName);
        ParameterName = parameterName;
    }
}
