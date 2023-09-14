// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Rest.Testing;

/// <summary>
///     An <see cref="HttpMessageHandler" /> that can be used for mocking in <see cref="HttpTest" /> and other classes in
///     this package.
/// </summary>
internal sealed class HttpTestMessageHandler : HttpMessageHandler
{
    /// <summary>
    ///     The current index in <see cref="ResponseMessages" />.
    /// </summary>
    private int _currentIndex;

    /// <summary>
    ///     The response messages to return in order.
    /// </summary>
    internal List<HttpResponseMessage> ResponseMessages { get; private set; } = new();

    /// <summary>
    ///     More or less the main feature of the <see cref="HttpTest" /> feature. Mocking this functionality is usually a lot
    ///     of ceremony, so this will abstract all that away.
    /// </summary>
    /// <param name="request">
    ///     The request being sent. Honestly doesn't matter. The current index in <see cref="ResponseMessages" /> will be
    ///     returned no matter what.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see cref="HttpResponseMessage" /> as it is at this point in time.</returns>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!ResponseMessages.Any())
        {
            throw new InvalidOperationException("No response messages are configured.");
        }

        if (_currentIndex >= ResponseMessages.Count)
        {
            throw new InvalidOperationException("All response messages have been exhausted.");
        }

        var current = ResponseMessages[_currentIndex];
        _currentIndex++;
        var task = Task.FromResult(current);
        return task;
    }
}
