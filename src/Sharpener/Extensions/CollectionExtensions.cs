// The Sharpener project and Facefire license this file to you under the MIT license.
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
    /// Adds a member to an array using <see cref="Span{T}"/>.
    /// </summary>
    /// <remarks>
    /// This returns the result, it does not modify your input.
    /// This is significantly more performant than converting to a <see cref="List{T}"/>, which is the general advice when working with arrays that need to be manipulated.
    /// </remarks>
    /// <param name="array">The array to add the member to.</param>
    /// <param name="member">The member to add to the array.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] Add<T>(this T[] array, T member)
    {
        var span = new Span<T>(array);
        var newArray = new T[array.Length + 1];
        span.CopyTo(newArray);
        newArray[array.Length] = member;
        return newArray;
    }
    /// <summary>
    /// Removes the first matching member of an array using <see cref="Span{T}"/>.
    /// </summary>
    /// <remarks>
    /// This returns the result, it does not modify your input.
    /// This is significantly more performant than converting to a <see cref="List{T}"/>, which is the general advice when working with arrays that need to be manipulated.
    /// </remarks>
    /// <param name="array">The array to remove a member from.</param>
    /// <param name="member">The member to remove the first instance of from the array.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] Remove<T>(this T[] array, T member)
    {
        var arraySpan = new Span<T>(array);
        var newArray = new T[array.Length - 1];
        var index = Array.IndexOf(array, member);
        if (index >= 0)
        {
            arraySpan[..index].CopyTo(newArray);
            arraySpan[(index + 1)..].CopyTo(newArray.AsSpan()[index..]);
        }
        return newArray;
    }
    /// <summary>
    /// Removes all matching instances from the array using <see cref="Span{T}"/>.
    /// </summary>
    /// /// <remarks>
    /// This returns the result, it does not modify your input.
    /// This is significantly more performant than converting to a <see cref="List{T}"/>, which is the general advice when working with arrays that need to be manipulated.
    /// </remarks>
    /// <param name="array">The array to remove all instances from.</param>
    /// <param name="member">What to remove all instances of.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] RemoveAll<T>(this T[] array, T member)
    {
        var writeIndex = 0;
        var arraySpan = new Span<T>(array);
        for (var i = 0; i < arraySpan.Length; i++)
        {
            if (!(arraySpan[i]!.Equals(member)))
            {
                arraySpan[writeIndex] = arraySpan[i];
                writeIndex++;
            }
        }
        return arraySpan[..writeIndex].ToArray();
    }
    /// <summary>
    /// Adds a range of members to an array using <see cref="Span{T}"/>.
    /// </summary>
    /// <remarks>
    /// This returns the result, it does not modify your input.
    /// This is significantly more performant than converting to a <see cref="List{T}"/>, which is the general advice when working with arrays that need to be manipulated.
    /// </remarks>
    /// <param name="array">The array to add the members to.</param>
    /// <param name="members">The membersto add to the array.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] AddRange<T>(this T[] array, IEnumerable<T> members)
    {
        var arraySpan = new Span<T>(array);
        var membersSpan = new Span<T>(members.AsArray());
        var newArray = new T[array.Length + membersSpan.Length];
        arraySpan.CopyTo(newArray);
        membersSpan.CopyTo(newArray.AsSpan()[array.Length..]);
        return newArray;
    }
    /// <summary>
    /// Perform an action on each member of an enumerable. Uses <see cref="Span{T}"/> for performance.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForAll<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        var asSpan = CollectionsMarshal.AsSpan(enumerable.AsList());
        ref var searchSpace = ref MemoryMarshal.GetReference(asSpan);
        for (var i = 0; i < asSpan.Length; i++)
        {
            var item = Unsafe.Add(ref searchSpace, i);
            action(item);
        }
    }
    /// <summary>
    /// Creates an array from the enumerable only if it is not already an array. Otherwise, simply returns it casted as an array.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] AsArray<T>(this IEnumerable<T> enumerable) => enumerable.IsArray() ? (T[])enumerable : enumerable.ToArray();
    /// <summary>
    /// Creates a list from the enumerable only if it is not already a list. Otherwise, simply returns it casted as a list.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> AsList<T>(this IEnumerable<T> enumerable) => enumerable.IsList() ? (List<T>)enumerable : enumerable.ToList();
    /// <summary>
    /// Checks if the enumerable is an array or not.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsArray<T>(this IEnumerable<T> enumerable) => enumerable.GetType() == typeof(T[]);
    /// <summary>
    /// Checks if the enumerable is a list or not.
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsList<T>(this IEnumerable<T> enumerable) => enumerable.GetType() == typeof(List<T>);
}
