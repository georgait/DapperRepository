using Repository.Domain.Exceptions;

namespace Repository.Core;

/// <summary>
///     Unit of work implementation of <see cref="IUnitOfWork"/>.
/// </summary>
/// <remarks>
///     <para>
///         The service lifetime is <see cref="ServiceLifetime.Scoped"/>. 
///     </para>
/// </remarks>
public sealed class UnitOfWork : IUnitOfWork
{   
    private readonly ILogger<UnitOfWork> _logger; 
    private readonly Guid _id = Guid.Empty;

    private Guid _transactionId = Guid.Empty;
    private IDbConnection _connection = null!;
    private IDbTransaction _transaction = null!;

    /// <summary>
    ///     Creates a new <see cref="UnitOfWork"/> instance.
    /// </summary>
    /// <param name="connection">Contains the db connection instance</param>
    /// <param name="logger">The logger of <see cref="UnitOfWork"/></param>
    public UnitOfWork(IDbConnection connection, ILogger<UnitOfWork> logger)
    {
        _id = Guid.NewGuid();
        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public Guid Id 
    { 
        get { return _id; }
    }

    /// <inheritdoc/>
    public Guid TransactionId
    { 
        get { return _transactionId; } 
    }

    /// <inheritdoc/>
    public IDbConnection Connection 
    { 
        get => _connection;
    }

    /// <inheritdoc/>
    public IDbTransaction Transaction 
    {
        get => _transaction; 
    }

    /// <inheritdoc/>
    public async Task TrackChangesAsync(Func<Task> dbActions)
    {
        if (_transaction is not null)
        {
            throw new NotNullTransactionException();
        }
        
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }
        
        _transaction = _connection.BeginTransaction();
        _transactionId = Guid.NewGuid();

        _logger.LogInformation("{id}: Transaction started", _transactionId);

        try
        {
            await dbActions();
            _transaction.Commit();
            _logger.LogInformation("{id}: Changes saved", _transactionId);
        }
        catch
        {
            _transaction.Rollback();
            _logger.LogWarning("{id}: Transaction rolled back", _transactionId);
            throw;
        }
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) return;

        if (_transaction is not null)
        {
            _transaction.Dispose();
            _transaction = null!;
            _logger.LogInformation("{id}: Transaction Disposed", _transactionId);
        }

        if (_connection is not null)
        {
            _connection.Dispose();
            _connection = null!;
            _logger.LogInformation("DB Connection Disposed");
        }
    }
}
