// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Options;

namespace Sharpener.Rest.Pagination;

/// <summary>
///     A paginated cursor for endpoints that might require repeat calls to get the full listing.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class PaginatedCursor<T>
{
    private readonly Func<int, int, Task<Option<Paginated<T>, HttpResponseMessage>>> _func;
    private readonly int _pageSize;
    private int _currentPage;
    private bool _hasMore;

    /// <summary>
    ///     Creates a new paginated response cursor that can be iteratively walked through.
    /// </summary>
    /// <param name="startPage">The start page of the request.</param>
    /// <param name="pageSize">The amount of items per page.</param>
    /// <param name="func">
    ///     The action to perform in order to obtain the response. Note that the two int parameters are the
    ///     current page and pageSize within the context of the call, not the constructor (retaining changes to them that may
    ///     take place).
    /// </param>
    public PaginatedCursor(int? startPage, int? pageSize,
        Func<int, int, Task<Option<Paginated<T>, HttpResponseMessage>>> func)
    {
        _hasMore = true;
        _currentPage = startPage ?? 1;
        _pageSize = pageSize ?? 0;
        _func = func;
    }

    /// <summary>
    ///     Advances the enumerator synchronously to the next element of the collection.
    /// </summary>
    /// <returns>True if advanced to the next element or false if it reaches the end.</returns>
#pragma warning disable CA2012
    public bool MoveNext => MoveNextAsync().ConfigureAwait(false).GetAwaiter().GetResult();
#pragma warning restore CA2012

    /// <summary>
    ///     The current element in the cursor.
    /// </summary>
    public Option<Paginated<T>, HttpResponseMessage> Current { get; private set; }

    /// <summary>
    ///     Disposes the cursor.
    /// </summary>
    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.CompletedTask);
    }

    /// <summary>
    ///     Advances the enumerator asynchronously to the next element of the collection.
    /// </summary>
    /// <returns>True if advanced to the next element or false if it reaches the end.</returns>
    public async ValueTask<bool> MoveNextAsync()
    {
        if (!_hasMore)
        {
            return false;
        }

        var result = await _func(_currentPage, _pageSize).ConfigureAwait(false);

        return result.Resolve(paginated =>
        {
            Current = paginated;
            _hasMore = paginated is not null;
            if (!_hasMore)
            {
                return false;
            }

            _currentPage++;
            return true;
        }, _ => false);
    }
}
