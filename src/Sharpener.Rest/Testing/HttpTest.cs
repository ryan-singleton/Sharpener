// The Sharpener project licenses this file to you under the MIT license.

using System.Net;
using Sharpener.Json.Extensions;

namespace Sharpener.Rest.Testing;

/// <summary>
///     A utility that allows for easier mocking of responses from <see cref="HttpClient" /> references.
/// </summary>
public sealed class HttpTest : IDisposable
{
    private readonly HttpTestMessageHandler _httpTestMessageHandler;

    /// <summary>
    ///     The <see cref="HttpClient" /> that will return whatever responses is configured for the test.
    /// </summary>
    public readonly HttpClient Client;

    /// <summary>
    ///     Creates a new test utility with a new handler and <see cref="HttpClient" />.
    /// </summary>
    /// <param name="baseUrl" />
    /// The url that will be put into the
    /// <see cref="HttpClient.BaseAddress" />
    /// . By default, this will be localhost, so leave it alone if that works./param>
    public HttpTest(string baseUrl = "https://localhost")
    {
        _httpTestMessageHandler = new HttpTestMessageHandler();
        Client = new HttpClient(_httpTestMessageHandler) { BaseAddress = new Uri(baseUrl) };
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Client.Dispose();
        _httpTestMessageHandler.Dispose();
    }

    /// <summary>
    ///     Clears out any existing responses.
    /// </summary>
    public void ClearResponses()
    {
        _httpTestMessageHandler.ResponseMessages.Clear();
    }

    /// <summary>
    ///     The response messages will be returned in order with subsequent <see cref="Client" /> send requests. The
    ///     <see cref="HttpMethod" /> will not matter as they call the <see cref="HttpClient" /> "SendAsync" method, which is
    ///     what is mocked.
    /// </summary>
    /// <remarks>
    ///     This does not clear out any existing responses. Call <see cref="ClearResponses" /> first if you want to start
    ///     fresh.
    /// </remarks>
    /// <param name="responseMessages">
    ///     The desired responses to receive, in order, next time the <see cref="Client" /> sends a
    ///     request.
    /// </param>
    public void RespondWith(params HttpResponseMessage[] responseMessages)
    {
        _httpTestMessageHandler.ResponseMessages.AddRange(responseMessages);
    }

    /// <summary>
    ///     A helper method that pre-configures a default <see cref="HttpResponseMessage" />, but then sets the item as a JSON
    ///     string as the content payload. By default, the returned status code will be 200 OK, but this can be overridden.
    /// </summary>
    /// <remarks>
    ///     Call this multiple times to set up multiple responses. They will be returned in order.
    /// </remarks>
    /// <param name="item">What to serialize to JSON and return in the response content.</param>
    /// <param name="code">The status code to return. Leave alone if 200 OK is .. ok. Ok?</param>
    /// <typeparam name="T">The type of item being serialized into the response content.</typeparam>
    public void RespondWithJson<T>(T? item, HttpStatusCode code = HttpStatusCode.OK)
    {
        var response = new HttpResponseMessage(code);
        if (item is not null)
        {
            response.Content = new StringContent(item.WriteJson());
        }

        _httpTestMessageHandler.ResponseMessages.Add(response);
    }
}
