namespace Repository.Core;

/// <summary>
///     Sql Server-specific implementation of <see cref="IDbContext"/>.
/// </summary>
/// <remarks>
///     <para>
///         The service lifetime is <see cref="ServiceLifetime.Scoped"/>. 
///     </para>
/// </remarks>
public class SqlServerContext : IDbContext
{
    private IUnitOfWork _unitOfWork;
    private readonly ILogger<SqlServerContext> _logger;

    /// <summary>
    ///     Creates a new <see cref="SqlServerContext"/> instance
    /// </summary>
    /// <param name="unitOfWork">Contains the unit of work instance.</param>
    /// <param name="logger">The logger of <see cref="SqlServerContext"/>></param>
    public SqlServerContext(IUnitOfWork unitOfWork,
                            ILogger<SqlServerContext> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    ///     Getter for the unit of work private field
    /// </summary>
    public IUnitOfWork UnitOfWork
    {
        get { return _unitOfWork; }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _unitOfWork.Dispose();
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        _logger.LogInformation("Sql Server Context disposed");
    }
}
