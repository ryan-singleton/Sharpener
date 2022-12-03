namespace Sharpener.Extensions;

/// <summary>
/// Extensions for collections of all types.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Perform an action on each member of an enumerable. Uses <see cref="Span{T}"/> for performance.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForAll<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        Span<T> asSpan = enumerable.AsArray();

        for (var i = 0; i < asSpan.Length; i++)
        {
            action(asSpan[i]);
        }
    }

    /// <summary>
    /// Creates an array from the enumerable only if it is not already an array. Otherwise, simply returns it casted as an array.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] AsArray<T>(this IEnumerable<T> enumerable) => enumerable.GetType() == typeof(T[]) ? (T[])enumerable : enumerable.ToArray();

    /// <summary>
    /// Creates a list from the enumerable only if it is not already a list. Otherwise, simply returns it casted as a list.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IList<T> AsList<T>(this IEnumerable<T> enumerable) => enumerable.GetType() == typeof(IList<T>) ? (IList<T>)enumerable : enumerable.ToList();
}
