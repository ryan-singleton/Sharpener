using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sharpener.Extensions;

/// <summary>
/// Extensions for collections of all types.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    ///Correlates the elements of two sequences based on matching keys. The default equality comparer is used to compare keys. Returns null inner results.
    /// </summary>
    /// <param name="outer">The first sequence to join.</param>
    /// <param name="inner">The sequence to join to the first sequence.</param>
    /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
    /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
    /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <returns>An <see cref="IEnumerable"/> that has elements of type <see cref="TResult"/> that are obtained by performing a left join on two sequences.</returns>
    public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer,
    IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) =>
        from outerMember in outer
        join innerMember in inner on outerKeySelector(outerMember) equals innerKeySelector(innerMember) into joins
        from joined in joins.DefaultIfEmpty()
        select resultSelector(outerMember, joined);


    /// <summary>
    /// Perform an action on each member of an enumerable. Uses <see cref="Span{T}"/> for performance.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForAll<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        Span<T> asSpan = CollectionsMarshal.AsSpan(enumerable.AsList());
        ref T searchSpace = ref MemoryMarshal.GetReference(asSpan);

        for (int i = 0; i < asSpan.Length; i++)
        {
            T? item = Unsafe.Add(ref searchSpace, i);
            action(item);
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
    public static List<T> AsList<T>(this IEnumerable<T> enumerable) => enumerable.GetType() == typeof(List<T>) ? (List<T>)enumerable : enumerable.ToList();
}
