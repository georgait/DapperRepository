namespace Repository.Contracts;

/// <summary>
///     A base generic repository. Implement this interface to have in one place 
///     the Read and Write functionalities.
/// </summary>
/// <typeparam name="TEntity">The type of entity</typeparam>
public interface IBaseRepository<TEntity> 
    where TEntity : class
{
    /// <summary>
    ///     <para>
    ///         The basic Write to DB method as async operation.
    ///         Use this method to insert, update or delete data to the database.
    ///     </para>
    /// </summary>
    /// <typeparam name="TParameters">The type of parameters.</typeparam>
    /// <param name="query">The sql query or the name of stored-procedure.</param>
    /// <param name="parameters">The parameters that are finally passed to the sql query.</param>
    /// <param name="commandType">The <see cref="CommandType"/></param>
    /// <returns></returns>
    Task<int> WriteAsync<TParameters>(string query, TParameters parameters, CommandType commandType = CommandType.Text);

    /// <summary>
    ///     <para>
    ///         The basic Read method as async operation.
    ///         Use this method to read data from the database.
    ///     </para>
    /// </summary>
    /// <typeparam name="TParameters">The type of parameters.</typeparam>
    /// <param name="query">The sql query or the name of stored-procedure.</param>
    /// <param name="parameters">The parameters that are finally passed to the sql query</param>
    /// <param name="commandType">The <see cref="CommandType"/></param>
    /// <returns></returns>
    Task<IReadOnlyList<TEntity>> ReadAsync<TParameters>(string query, TParameters parameters, CommandType commandType = CommandType.Text);

    /// <summary>
    ///     <para>
    ///         The basic Read method as async operation.
    ///         Use this method to read data in one-to-many relationship scenarios from the database.
    ///     </para>
    /// </summary>
    /// <typeparam name="TNestedEntity">The type of child entity</typeparam>
    /// <typeparam name="TParameters">The type of parameters.</typeparam>
    /// <param name="query">The sql query or the name of stored-procedure.</param>
    /// <param name="parameters">The parameters that are finally passed to the sql query</param>
    /// <param name="commandType">The <see cref="CommandType"/></param>
    /// <returns></returns>
    Task<IReadOnlyList<TEntity>> ReadAsync<TNestedEntity, TParameters>(string query, TParameters parameters, CommandType commandType = CommandType.Text);    
}
