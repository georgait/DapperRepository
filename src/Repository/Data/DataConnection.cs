namespace Repository.Data;

/// <summary>
/// Provides methods that enables different database engines.
/// </summary>
public class DataConnection
{
    /// <summary>
    ///     Use this method if your database engine is SqlServer.
    /// </summary>
    /// <param name="connectionString">The current connection string.</param>
    /// <returns><see cref="IDbConnection"/></returns>
    public IDbConnection UseSqlServer(string connectionString) =>
           new SqlConnection(connectionString);


    /// <summary>
    ///     Use this method if your database engine is SQLite.
    /// </summary>
    /// <param name="connectionString">The current connection string.</param>
    /// <returns><see cref="IDbConnection"/></returns>
    public IDbConnection UseSqlLite(string connectionString) =>
        new SqliteConnection(connectionString);
}
