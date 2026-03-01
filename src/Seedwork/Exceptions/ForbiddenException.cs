namespace Seedwork.Exceptions;

/// <summary>
/// Thrown when an operation is not permitted for the current user/context.
/// </summary>
public class ForbiddenException : DomainException
{
    public ForbiddenException(string message)
        : base(message)
    {
    }
}
