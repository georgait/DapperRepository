namespace Repository;

/// <summary>
///     A class that helps to configure service to DI.
/// </summary>
public static class ServiceRegistration
{    
    /// <summary>
    ///     Use this method to configure automatically the services.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="dbConnection">A func that takes in the <see cref="DataConnection"/> and returns the <see cref="IDbConnection"/>.</param>
    public static void AddDapperRepository(this IServiceCollection services, Func<DataConnection, IDbConnection> dbConnection)
    {
        services.AddScoped(_ => 
            dbConnection.Invoke(new DataConnection()));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDbContext, SqlServerContext>();
    }
}
