using static Repository.Helpers.EntitiesMapper;

namespace Repository;

/// <summary>
///     Implementation of <see cref="IBaseRepository{TEntity}"/> 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
     where TEntity : class
{
    private readonly IDbContext _context;

    /// <summary>
    ///     The constructor of the abstract class <see cref="BaseRepository{TEntity}"/>.
    /// </summary>
    /// <param name="context">The db context (eg SqlServerContext).</param>
    protected BaseRepository(IDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<int> WriteAsync<TParameters>(string query, TParameters parameters, CommandType commandType = CommandType.Text)
    {   
        var result = await _context.UnitOfWork.Connection.QuerySingleOrDefaultAsync<int>(query,
           parameters,
           transaction: _context.UnitOfWork.Transaction,
           commandType: commandType);

        return result;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> ReadAsync<TParameters>(string query, TParameters parameters, CommandType commandType = CommandType.Text)
    {
        var result = await _context.UnitOfWork.Connection.QueryAsync<TEntity>(query,
            parameters,
            transaction: _context.UnitOfWork.Transaction,
            commandType: commandType);

        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> ReadAsync<TNestedEntity, TParameters>(string query, TParameters parameters, CommandType commandType = CommandType.Text)
    {
        var lookup = new Dictionary<int, TEntity>();
        var result = await _context.UnitOfWork.Connection.QueryAsync<TEntity, TNestedEntity, TEntity>(query,
            (one, many) => Map(one, many, lookup),
            parameters,
            transaction: _context.UnitOfWork.Transaction,
            commandType: commandType);

        return result.Distinct().ToList();
    }
}
