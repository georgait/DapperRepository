namespace Repository.Helpers;

/// <summary>
///     internal class that supports the DapperRepository to handle multi-mapping scenarios.
/// </summary>
internal static class EntitiesMapper
{
    /// <summary>
    ///     Creates a map between a parent and a child entity via reflection.
    ///     The nested (child) entity represents the "MANY" of a one-to-many relationship.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TNestedEntity"></typeparam>
    /// <param name="one"></param>
    /// <param name="many"></param>
    /// <param name="lookup"></param>
    /// <returns></returns>
    public static TEntity Map<TEntity, TNestedEntity>(TEntity one, TNestedEntity many, IDictionary<int, TEntity> lookup)
    {
        var id = one.GetId();

        if (!lookup.TryGetValue(id, out var currentOne))
        {
            currentOne = one;
            lookup.Add(id, currentOne);
        }

        var collection = GetList<TEntity, TNestedEntity>(one, currentOne);

        collection.AddEntity<TEntity, TNestedEntity>(many);

        return currentOne;
    }

    private static List<TNestedEntity> GetList<TEntity, TNestedEntity>(TEntity entity, TEntity? current)
    {
        var manyProperty = entity!.GetType().GetProperties()
            .FirstOrDefault(p => p.IsDefined(typeof(ManyRelationshipAttribute), true))!;

        return (List<TNestedEntity>)manyProperty?.GetValue(current)!;
    }
}
