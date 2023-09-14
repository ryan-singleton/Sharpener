// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Rest.Pagination;

/// <summary>
///     A container for responses that are paginated.
/// </summary>
/// <typeparam name="T">The type of items on the page.</typeparam>
public sealed class Paginated<T>
{
    /// <summary>
    ///     The items on the current page.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();

    /// <summary>
    ///     The page that the loaded items belong to.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    ///     Whether there are more items on further pages.
    /// </summary>
    public bool HasMore { get; set; }

    /// <summary>
    ///     What the next page will be. Null if this is the last page.
    /// </summary>
    public int? NextPage => HasMore ? CurrentPage + 1 : null;
}
