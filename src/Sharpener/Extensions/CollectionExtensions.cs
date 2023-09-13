// The Sharpener project licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sharpener.Extensions;

/// <summary>
///     Extensions for collections of all types.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    ///     Adds an object to the end of the <see cref="Array" />. Uses <see cref="Span{T}" /> for performance.
    /// </summary>
    /// <remarks>
    ///     This returns the modified array, it does not modify the input.
    ///     This is significantly more performant than converting to a <see cref="List{T}" /> but only when the array is not
    ///     commonly added to or removed from. Otherwise, converting to a list is recommended.
    /// </remarks>
    /// <param name="array">The array to add the element to.</param>
    /// <param name="element">The element to add to the array.</param>
    /// <typeparam name="T"> The type of the elements in the array.</typeparam>
    /// <returns> The modified array.</returns>
    public static T[] Add<T>(this T[] array, T element)
    {
        var arraySpan = new ReadOnlySpan<T>(array);
        var newArray = new T[arraySpan.Length + 1];
        arraySpan.CopyTo(newArray);
        newArray[arraySpan.Length] = element;
        return newArray;
    }

    /// <summary>
    ///     Adds the elements of the specified collection to the end of the <see cref="Array" />. Uses <see cref="Span{T}" />
    ///     for performance.
    /// </summary>
    /// <remarks>
    ///     This returns the modified array, it does not modify the input.
    ///     This is significantly more performant than converting to a <see cref="List{T}" /> but only when the array is not
    ///     commonly added to or removed from. Otherwise, converting to a list is recommended.
    /// </remarks>
    /// <param name="array">The array to add the elements to.</param>
    /// <param name="elements">The elements to add to the array.</param>
    /// <typeparam name="T"> The type of the elements in the array.</typeparam>
    /// <returns> The modified array.</returns>
    public static T[] AddRange<T>(this T[] array, IEnumerable<T> elements)
    {
#if NET5_0_OR_GREATER
        var arraySpan = new ReadOnlySpan<T>(array);
        var membersSpan = new Span<T>(elements.AsArray());
        var newArray = new T[arraySpan.Length + membersSpan.Length];
        arraySpan.CopyTo(newArray);
        membersSpan.CopyTo(newArray.AsSpan()[arraySpan.Length..]);
        return newArray;
#endif

#if NETSTANDARD2_0_OR_GREATER
        return array.Concat(elements).ToArray();
#endif
    }

    /// <summary>
    ///     Creates an array from the enumerable only if it is not already an array. Otherwise, simply returns it casted as an
    ///     array.
    /// </summary>
    /// <param name="enumerable"> The enumerable to cast or convert to an array.</param>
    /// <typeparam name="T"> The type of the elements in the array.</typeparam>
    /// <returns> The enumerable as an array.</returns>
    public static T[] AsArray<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.GetType() == typeof(T[]) ? (T[])enumerable : enumerable.ToArray();
    }

    /// <summary>
    ///     Creates a list from the enumerable only if it is not already a list. Otherwise, simply returns it casted as a list.
    /// </summary>
    /// <param name="enumerable"> The enumerable to cast or convert to a list.</param>
    /// <typeparam name="T"> The type of the elements in the list.</typeparam>
    /// <returns> The enumerable as a list.</returns>
    public static List<T> AsList<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.GetType() == typeof(List<T>) ? (List<T>)enumerable : enumerable.ToList();
    }

    /// <summary>
    ///     Perform an action on each member of an enumerable. Uses <see cref="Span{T}" /> for performance.
    /// </summary>
    /// <param name="enumerable"> The enumerable to perform the action on.</param>
    /// <param name="action">   The action to perform on each member.</param>
    /// <typeparam name="T"> The type of the elements in the enumerable.</typeparam>
    public static void ForAll<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
#if NET5_0_OR_GREATER
        var asSpan = CollectionsMarshal.AsSpan(enumerable.AsList());
        ref var searchSpace = ref MemoryMarshal.GetReference(asSpan);
        for (var i = 0; i < asSpan.Length; i++)
        {
            var item = Unsafe.Add(ref searchSpace, i);
            action(item);
        }
#endif

#if NETSTANDARD2_0_OR_GREATER
        var asSpan = enumerable.AsArray().AsSpan();
        for (int i = 0; i < asSpan.Length; i++)
        {
            action(asSpan[i]);
        }
#endif
    }

    /// <summary>
    ///     Checks if the enumerable is an array or not.
    /// </summary>
    /// <param name="enumerable"> The enumerable to check.</param>
    /// <typeparam name="T"> The type of the elements in the enumerable.</typeparam>
    /// <returns> True if the enumerable is an array, false otherwise.</returns>
    public static bool IsArray<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.GetType() == typeof(T[]);
    }

    /// <summary>
    ///     Checks if the enumerable is a list or not.
    /// </summary>
    /// <param name="enumerable"> The enumerable to check.</param>
    /// <typeparam name="T"> The type of the elements in the enumerable.</typeparam>
    /// <returns> True if the enumerable is a list, false otherwise.</returns>
    public static bool IsList<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.GetType() == typeof(List<T>);
    }

    /// <summary>
    ///     Returns true if the enumerable is null or empty.
    /// </summary>
    /// <param name="enumerable">
    ///     The enumerable to check.
    /// </param>
    /// <typeparam name="T">
    ///     The type of the enumerable.
    /// </typeparam>
    /// <returns>
    ///     <c>true</c> if the enumerable is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
    {
        if (enumerable is null)
        {
            return true;
        }

        return enumerable switch
        {
            T[] array => array.Length == 0,
            ICollection<T> collection => collection.Count == 0,
            _ => !enumerable.Any()
        };
    }

    /// <summary>
    ///     Correlates the elements of two sequences based on matching keys. The default equality comparer is used to compare
    ///     keys. Returns null inner results.
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
    /// <returns>
    ///     An <see cref="IEnumerable{T}" /> that has elements of result type that are obtained by
    ///     performing a left join on two sequences.
    /// </returns>
    public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer,
        IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector)
    {
        return from outerMember in outer
            join innerMember in inner on outerKeySelector(outerMember) equals innerKeySelector(innerMember) into joins
            from joined in joins.DefaultIfEmpty()
            select resultSelector(outerMember, joined);
    }

    /// <summary>
    ///     Removes the first occurrence of a specific object from the <see cref="Array" />. Uses <see cref="Span{T}" /> for
    ///     performance.
    /// </summary>
    /// <remarks>
    ///     This returns the modified array, it does not modify the input.
    ///     This is significantly more performant than converting to a <see cref="List{T}" /> but only when the array is not
    ///     commonly added to or removed from. Otherwise, converting to a list is recommended.
    /// </remarks>
    /// <param name="array">The array to remove an element from.</param>
    /// <param name="element">The element to remove the first instance of from the array.</param>
    /// <typeparam name="T"> The type of the elements in the array.</typeparam>
    /// <returns> The modified array.</returns>
    public static T[] Remove<T>(this T[] array, T element)
    {
#if NET5_0_OR_GREATER
        var arraySpan = new ReadOnlySpan<T>(array);
        var newArray = new T[arraySpan.Length - 1];
        var index = Array.IndexOf(array, element);
        if (index < 0)
        {
            return newArray;
        }

        arraySpan[..index].CopyTo(newArray);
        arraySpan[(index + 1)..].CopyTo(newArray.AsSpan()[index..]);
        return newArray;
#endif

#if NETSTANDARD2_0_OR_GREATER
        var newArray = new T[array.Length - 1];
        var index = Array.IndexOf(array, element);
        if (index < 0)
        {
            return newArray;
        }

        var source = array.AsSpan();
        var destination = newArray.AsSpan();
        source.Slice(0, index).CopyTo(destination);
        source.Slice(index + 1).CopyTo(destination.Slice(index));
        return newArray;
#endif
    }

    /// <summary>
    ///     Removes all the elements that match the conditions defined by the specified predicate. Uses <see cref="Span{T}" />
    ///     for performance.
    /// </summary>
    /// <remarks>
    ///     This returns the modified array, it does not modify the input.
    ///     This is significantly more performant than converting to a <see cref="List{T}" /> but only when the array is not
    ///     commonly added to or removed from. Otherwise, converting to a list is recommended.
    /// </remarks>
    /// <param name="array">The array to remove all instances from.</param>
    /// <param name="action">The conditions for removal.</param>
    /// <typeparam name="T"> The type of the elements in the array.</typeparam>
    /// <returns> The modified array.</returns>
    public static T[] RemoveAll<T>(this T[] array, Func<T, bool> action)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

#if NET5_0_OR_GREATER
        var writeIndex = 0;
        var arraySpan = new Span<T>(array);
        for (var i = 0; i < arraySpan.Length; i++)
        {
            if (action(arraySpan[i]))
            {
                continue;
            }

            arraySpan[writeIndex++] = arraySpan[i];
        }

        return arraySpan[..writeIndex].ToArray();
#endif

#if NETSTANDARD2_0_OR_GREATER
        var writeIndex = 0;
        var newArray = new T[array.Length];

        for (var i = 0; i < array.Length; i++)
        {
            if (action(array[i]))
            {
                continue;
            }
            newArray[writeIndex++] = array[i];
        }

        return newArray.AsSpan(0, writeIndex).ToArray();
#endif
    }
}
