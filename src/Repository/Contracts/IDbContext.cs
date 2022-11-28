namespace Repository.Contracts;

/// <summary>
///     The general DB context interface
/// </summary>
public interface IDbContext : IDisposable
{
    /// <summary>
    ///     Unit of Work getter.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}