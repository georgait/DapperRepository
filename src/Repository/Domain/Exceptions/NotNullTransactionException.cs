namespace Repository.Domain.Exceptions;

/// <summary>
///     Exception intended to be thrown when a non-empty transaction occurs.
/// </summary>
public class NotNullTransactionException : Exception
{
    /// <summary>
    /// Creates a new <see cref="NotNullTransactionException"/> instance.
    /// </summary>
    public NotNullTransactionException() 
        : base("Transaction must be null here")
    {
    }
}
