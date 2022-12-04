using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sharpener.Extensions;

/// <summary>
/// Extensions for collections of all types.
/// </summary>
public static class CollectionExtensions
{
    public static (IEnumerable<TInner> Inner, IEnumerable<TOuter> Outer) Join<TInner, TOuter>(this IEnumerable<TInner> inner, IEnumerable<TOuter> outer) => (inner, outer);

    public static (TInner Inner, TOuter? Outer)? On<TInner, TOuter>(this (IEnumerable<TInner> Inner, IEnumerable<TOuter> Outer) join, Func<TInner, TOuter, bool> predicate)
    {
        Span<TInner> innerAsSpan = join.Inner.AsList();
        Span<TOuter> outerAsSpan = join.Outer.AsList();


        var query = from inner in innerAsSpan
                    join outer in outerAsSpan on predicate(inner, outer) equals true

                    return null;
    }

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
