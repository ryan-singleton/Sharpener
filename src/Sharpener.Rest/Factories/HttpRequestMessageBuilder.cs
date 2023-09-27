// The Sharpener project licenses this file to you under the MIT license.

using System.Net.Http.Headers;

namespace Sharpener.Rest.Factories;

/// <summary>
///     A builder for <see cref="HttpRequestMessage" />.
/// </summary>
internal sealed class HttpRequestMessageBuilder
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HttpRequestMessageBuilder" /> class.
    /// </summary>
    internal HttpRequestMessageBuilder()
    {
        Headers = Activator.CreateInstance(typeof(HttpRequestHeaders), nonPublic: true) as HttpRequestHeaders
                  ?? throw new InvalidOperationException();
    }

    /// <summary>
    ///     The content to send with the request.
    /// </summary>
    internal HttpContent? Content { get; set; }

    /// <summary>
    ///     The headers to send with the request.
    /// </summary>
    internal HttpRequestHeaders Headers { get; set; }

    /// <summary>
    ///     Builds a <see cref="HttpRequestMessage" /> from the template.
    /// </summary>
    /// <param name="httpMethod"> The HTTP method to use.</param>
    /// <param name="requestUri"> The URI to send the request to.</param>
    /// <returns> The <see cref="HttpRequestMessage" /> built from the template.</returns>
    internal HttpRequestMessage Build(HttpMethod httpMethod, Uri requestUri)
    {
        var request = new HttpRequestMessage(httpMethod, requestUri);
        if (Content != null)
        {
            request.Content = Content;
        }

        foreach (var header in Headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        return request;
    }
}
