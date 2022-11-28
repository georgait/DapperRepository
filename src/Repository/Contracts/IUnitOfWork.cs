namespace Repository.Contracts;

/// <summary>
/// Enables the principle of minimum calls to the database.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// The ID of the instance.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// The database connection
    /// </summary>
    IDbConnection Connection { get; }

    /// <summary>
    /// The database transaction
    /// </summary>
    IDbTransaction Transaction { get; }

    /// <summary>
    ///     <para>The transaction ID.</para>
    ///     <para>Use this ID for testing purposes to distinguish between transactions.</para>
    /// </summary>
    Guid TransactionId { get; }

    /// <summary>
    ///     Asynchronously tracks changes made to entities.
    /// </summary>
    /// <param name="dbActions">A func that returns a task. Use this func to encapsulate multiple write or read methods into a single transaction.</param>
    /// <returns><see cref="Task"/></returns>
    Task TrackChangesAsync(Func<Task> dbActions);
}
