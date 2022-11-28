namespace Repository.Helpers;

/// <summary>
/// Utility class that helps to parse metadata
/// </summary>
internal static class PropertyHelper
{
    private static readonly ConcurrentDictionary<string, Delegate> _cache = new();

    /// <summary>
    ///     Use this method to get the value of a property at runtime via reflection. 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of result.></typeparam>
    /// <param name="property">The property of the entity as <see cref="PropertyInfo"/></param>
    /// <returns><see cref="Func{T, TResult}"/></returns>
    public static Func<TEntity, TResult> InvokeGet<TEntity, TResult>(PropertyInfo property) =>
        (Func<TEntity, TResult>)_cache.GetOrAdd(property.Name, key =>
        {
            var getMethod = property.GetMethod;
            var res = getMethod?.CreateDelegate(typeof(Func<TEntity, TResult>))!;
            return res;
        });

    /// <summary>
    ///     Use this method to set a value to a property at runtime via reflection. 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TNestedEntity">The type of the child (or nested) entity.</typeparam>
    /// <param name="type">The type of the nested entity.</param>
    /// <returns><see cref="Action{T1, T2}"/></returns>
    public static Action<List<TNestedEntity>, TNestedEntity> InvokeAdd<TEntity, TNestedEntity>(Type type) =>
        (Action<List<TNestedEntity>, TNestedEntity>)_cache.GetOrAdd("Many", key =>
        {
            var addMethod = type.GetMethod("Add")!;

            var delegateType = typeof(Action<List<TNestedEntity>, TNestedEntity>)!;
            var res = addMethod?.CreateDelegate(delegateType)!;
            return (Action<List<TNestedEntity>, TNestedEntity>)res;
        });
}
