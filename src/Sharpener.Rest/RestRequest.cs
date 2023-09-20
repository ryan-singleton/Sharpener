// The Sharpener project licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Sharpener.Extensions;
using Sharpener.Rest.Extensions;
using Sharpener.Rest.Retry;

namespace Sharpener.Rest;

/// <summary>
///     A request type that can be built with fluent syntax.
/// </summary>
public sealed class RestRequest
{
    private readonly HttpClient _httpClient;

    /// <summary>
    ///     The request that will be built and sent.
    /// </summary>
    internal readonly HttpRequestMessage Request;

    /// <summary>
    ///     The builder of the <see cref="Uri" /> that will be used in the request.
    /// </summary>
    internal readonly UriBuilder UriBuilder;

    /// <summary>
    ///     Creates a <see cref="RestRequest" />.
    /// </summary>
    /// <param name="httpClient">
    ///     The <see cref="HttpClient" /> that will make the request in the end. It will not be affected
    ///     in any way by this class. It must have a base address assigned for use, or a base address override must be
    ///     supplied.
    /// </param>
    /// <param name="baseAddress">
    ///     The optional <see cref="HttpClient" /> base address override. If not supplied, the one in the
    ///     HttpClient will be used. Between both of those, one must contain a base address, however.
    /// </param>
    /// <exception cref="NullReferenceException">No base address was assigned to the <see cref="HttpClient" />.</exception>
    public RestRequest(HttpClient httpClient, Uri? baseAddress = null)
    {
        if (baseAddress is not null)
        {
            httpClient.BaseAddress = baseAddress;
        }

        if (httpClient.BaseAddress is null)
        {
            throw new NullReferenceException("The http client must have a base address");
        }

        _httpClient = httpClient;
        UriBuilder = new UriBuilder(httpClient.BaseAddress);
        Request = new HttpRequestMessage();
    }

    /// <summary>
    ///     Gets the current <see cref="Uri" /> of the request.
    /// </summary>
    public Uri CurrentUri => UriBuilder.Uri;

    /// <summary>
    ///     The options for retrying the request.
    /// </summary>
    internal RetryOptions? RetryOptions { get; private set; }

    /// <summary>
    ///     Adds the query parameters based upon an object and its properties. This is done with serialization and then
    ///     deserialization to a dictionary.
    /// </summary>
    /// <param name="values">The object defining the query parameters.</param>
    /// <returns> The <see cref="RestRequest" /> that is being configured.</returns>
    public RestRequest AddQueries(object values)
    {
        var dictionary = values.ToParameters<string, object>();
        if (dictionary is null)
        {
            return this;
        }

        foreach (var pair in dictionary)
        {
            AddQuery(pair.Key, pair.Value?.ToString());
        }

        return this;
    }

    /// <summary>
    ///     Adds the query parameter based on name and value. If either value is null, the query parameter is not added. Empty
    ///     strings can bypass this if desired.
    /// </summary>
    /// <param name="name">The name of the query parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <returns></returns>
    public RestRequest AddQuery(string name, object? value)
    {
        if (string.IsNullOrEmpty(name) || value is null)
        {
            return this;
        }

        var query = HttpUtility.ParseQueryString(UriBuilder.Query);
        query[name] = value.ToString();
        UriBuilder.Query = query.ToString();
        return this;
    }

    /// <summary>
    ///     Sends the request with a DELETE method.
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> DeleteAsync()
    {
        return await SendAsync(HttpMethod.Delete).ConfigureAwait(false);
    }

    /// <summary>
    ///     Sends the request with a GET method.
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> GetAsync()
    {
        return await SendAsync(HttpMethod.Get).ConfigureAwait(false);
    }

    /// <summary>
    ///     Sends the request with a PATCH method.
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> PatchAsync()
    {
        return await SendAsync(new HttpMethod("PATCH")).ConfigureAwait(false);
    }

    /// <summary>
    ///     Sends the request with a POST method.
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> PostAsync()
    {
        return await SendAsync(HttpMethod.Post).ConfigureAwait(false);
    }

    /// <summary>
    ///     Sends the request with a PUT method.
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> PutAsync()
    {
        return await SendAsync(HttpMethod.Put).ConfigureAwait(false);
    }

    /// <summary>
    ///     The logic that sends the request and provides a <see cref="HttpResponseMessage" />.
    /// </summary>
    /// <remarks>
    ///     Are you guys silly? I'm just gonna send it.
    /// </remarks>
    /// <param name="httpMethod">The http method of the request.</param>
    /// <returns> The <see cref="HttpResponseMessage" /> from the request.</returns>
    public async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod)
    {
        if (_httpClient is null)
        {
            throw new NullReferenceException(
                "The RestRequest must have an assigned HttpClient to perform Send operations.");
        }

        var func = () => _httpClient.SendAsync(new HttpRequestMessage(httpMethod, CurrentUri));
        if (RetryOptions is null)
        {
            return await func().ConfigureAwait(false);
        }

        return await func.WithRetry(RetryOptions).ConfigureAwait(false);
    }

    /// <summary>
    ///     Adds a basic token to the Authentication header.
    /// </summary>
    /// <param name="username">The username of the token.</param>
    /// <param name="password">The password of the token.</param>
    /// <returns> The <see cref="RestRequest" /> that is being configured.</returns>
    public RestRequest SetBasicToken(string username, string password)
    {
        Request.Headers.SetBasicToken(username, password);
        return this;
    }

    /// <summary>
    ///     Adds a Bearer token to the Authentication header.
    /// </summary>
    /// <param name="token">The bearer token.</param>
    /// <returns> The <see cref="RestRequest" /> that is being configured.</returns>
    public RestRequest SetBearerToken(string token)
    {
        Request.Headers.SetBearerToken(token);
        return this;
    }

    /// <summary>
    ///     Adds form URL encoded content to the request.
    /// </summary>
    /// <param name="values"> The object defining the form content.</param>
    /// <returns> The <see cref="RestRequest" /> that is being configured.</returns>
    public RestRequest SetFormContent(object values)
    {
        var dictionary = values.ToParameters<string, string>();
        if (dictionary is null)
        {
            return this;
        }

        Request.Content = new FormUrlEncodedContent(dictionary);

        return this;
    }

    /// <summary>
    ///     Adds a custom header to the request.
    /// </summary>
    /// <param name="key">The header key.</param>
    /// <param name="value">The value of the header.</param>
    /// <returns> The <see cref="RestRequest" /> that is being configured.</returns>
    public RestRequest SetHeader(string key, string value)
    {
        Request.Headers.Add(key, value);
        return this;
    }

    /// <summary>
    ///     Adds the headers based upon an object and its properties. This is done with serialization and then
    ///     deserialization to a dictionary.
    /// </summary>
    /// <param name="values"> The object defining the headers.</param>
    /// <returns> The <see cref="RestRequest" /> that is being configured.</returns>
    public RestRequest SetHeaders(object values)
    {
        var dictionary = values.ToParameters<string, object>();
        if (dictionary is null)
        {
            return this;
        }

        foreach (var pair in dictionary)
        {
            SetHeader(pair.Key, pair.Value?.ToString() ?? "");
        }

        return this;
    }

    /// <summary>
    ///     Adds the provided object as a JSON payload in the request.
    /// </summary>
    /// <param name="content">The request body.</param>
    /// <returns></returns>
    public RestRequest SetJsonContent(object? content)
    {
        if (content is null)
        {
            return this;
        }

        Request.Content = content.ToJsonStringContent();
        Request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
        return this;
    }

    /// <summary>
    ///     Adds values to the request path. They are concatenated with slashes. This will not overwrite previous calls to this
    ///     method.
    /// </summary>
    /// <param name="paths">The path values to add to the request url in the end.</param>
    /// <returns></returns>
    public RestRequest SetPaths(params string?[] paths)
    {
        paths = paths
            .Where(segment => !string.IsNullOrEmpty(segment))
            .Select(HttpUtility.UrlEncode)
            .AsArray();

        if (paths.IsNullOrEmpty())
        {
            return this;
        }

        UriBuilder.Path = string.Join("/", new[] { UriBuilder.Path.TrimEnd('/') }.Concat(paths));
        return this;
    }

    /// <summary>
    ///     Uses retry options for the request.
    /// </summary>
    /// <param name="configure">The retry configuration action.</param>
    /// <returns> The <see cref="RestRequest" /> that is being configured.</returns>
    public RestRequest UseRetry(Action<RetryOptions>? configure = null)
    {
        RetryOptions = new RetryOptions();
        configure?.Invoke(RetryOptions);
        return this;
    }

    /// <summary>
    ///     Adds the provided object as a string payload in the request.
    /// </summary>
    /// <param name="content">The request body.</param>
    /// <param name="encoding"> The encoding of the content. Defaults to UTF-8.</param>
    /// <param name="mediaType"> The media type of the content. Defaults to text/plain.</param>
    /// <returns> The current state of the REST request</returns>
    public RestRequest WithStringContent(string content, Encoding? encoding = null, string? mediaType = null)
    {
        if (string.IsNullOrEmpty(content))
        {
            return this;
        }

        mediaType ??= "text/plain";
        Request.Content = new StringContent(content, encoding ?? Encoding.UTF8, mediaType);
        Request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);
        return this;
    }
}
