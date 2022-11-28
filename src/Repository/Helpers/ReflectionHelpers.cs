namespace Repository.Helpers;

/// <summary>
/// Class that have extesnsion helper methods related to reflection
/// </summary>
internal static class ReflectionHelpers
{
    /// <summary>
    ///     Use this extension method to parse the ID of an entity at runtime.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity</param>
    /// <returns><see cref="int"/></returns>
    public static int GetId<TEntity>(this TEntity entity)
    {
        var entityType = entity!.GetType();
        var idProperty = entityType.GetProperties()
        .FirstOrDefault(p => p.IsDefined(typeof(PrimaryKeyAttribute), true))!;

        var getId = PropertyHelper.InvokeGet<TEntity, int>(idProperty);
        return getId(entity);
    }

    /// <summary>
    ///     Use this extension method to add item to the child entity list of a parent entity at runtime.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TNestedEntity">The type of the child entity.</typeparam>
    /// <param name="collection">The list of child entity that represents the many relationship.</param>
    /// <param name="entity">The child entity</param>
    public static void AddEntity<TEntity, TNestedEntity>(this List<TNestedEntity> collection, TNestedEntity entity)
    {
        var collectionType = collection.GetType();

        var addToCollection = PropertyHelper.InvokeAdd<TEntity, TNestedEntity>(collectionType);
        addToCollection(collection, entity);
    }
}
