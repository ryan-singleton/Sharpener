// The Sharpener project licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Sharpener.Extensions;
using Sharpener.Rest.Extensions;
using Sharpener.Rest.Factories;
using Sharpener.Rest.Retry;

namespace Sharpener.Rest;

/// <summary>
///     A request type that can be built with fluent syntax.
/// </summary>
public sealed class RestRequest : IRestRequest
{
    private readonly HttpClient _httpClient;

    /// <summary>
    ///     The request that will be built and sent.
    /// </summary>
    internal readonly HttpRequestMessageBuilder Request;

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
        Request = new HttpRequestMessageBuilder();
    }

    /// <inheritdoc />
    public Uri CurrentUri => UriBuilder.Uri;

    /// <summary>
    ///     The options for retrying the request.
    /// </summary>
    internal RetryOptions? RetryOptions { get; private set; }

    /// <inheritdoc />
    public IRestRequest AddQueries(object values)
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

    /// <inheritdoc />
    public IRestRequest AddQuery(string name, object? value)
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

    /// <inheritdoc />
    public async Task<HttpResponseMessage> DeleteAsync()
    {
        return await SendAsync(HttpMethod.Delete).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> GetAsync()
    {
        return await SendAsync(HttpMethod.Get).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PatchAsync()
    {
        return await SendAsync(new HttpMethod("PATCH")).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostAsync()
    {
        return await SendAsync(HttpMethod.Post).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PutAsync()
    {
        return await SendAsync(HttpMethod.Put).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod)
    {
        if (_httpClient is null)
        {
            throw new NullReferenceException(
                "The RestRequest must have an assigned HttpClient to perform Send operations.");
        }

        var func = () => _httpClient.SendAsync(Request.Build(httpMethod, CurrentUri));
        if (RetryOptions is null)
        {
            return await func().ConfigureAwait(false);
        }

        return await func.WithRetry(RetryOptions).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public IRestRequest SetBasicToken(string username, string password)
    {
        Request.Headers.SetBasicToken(username, password);
        return this;
    }

    /// <inheritdoc />
    public IRestRequest SetBearerToken(string token)
    {
        Request.Headers.SetBearerToken(token);
        return this;
    }

    /// <inheritdoc />
    public IRestRequest SetFormContent(object values)
    {
        var dictionary = values.ToParameters<string, string>();
        if (dictionary is null)
        {
            return this;
        }

        Request.Content = new FormUrlEncodedContent(dictionary);

        return this;
    }

    /// <inheritdoc />
    public IRestRequest SetHeader(string key, string value)
    {
        Request.Headers.Add(key, value);
        return this;
    }

    /// <inheritdoc />
    public IRestRequest SetHeaders(object values)
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

    /// <inheritdoc />
    public IRestRequest SetJsonContent(object? content)
    {
        if (content is null)
        {
            return this;
        }

        Request.Content = content.ToJsonStringContent();
        Request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
        return this;
    }

    /// <inheritdoc />
    [Obsolete("Use SetMultipartFormData with the delegate function instead.")]
    public IRestRequest SetMultipartFormData(object values)
    {
        SetMultipartFormData(content => content.AddStringParts(values));
        return this;
    }

    /// <inheritdoc />
    public IRestRequest SetMultipartFormData(Action<MultipartFormDataContent> buildContent,
        string? mediaType = "multipart/form-data", string boundary = "----WebKitFormBoundary7GuI94hQ253xT0v")
    {
        var content = new MultipartFormDataContent(boundary);
        buildContent(content);
        Request.Content = content;
        if (string.IsNullOrWhiteSpace(mediaType))
        {
            return this;
        }

        Request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(mediaType);
        return this;
    }

    /// <inheritdoc />
    public IRestRequest SetPaths(params string?[] paths)
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

    /// <inheritdoc />
    public IRestRequest UseRetry(Action<RetryOptions>? configure = null)
    {
        RetryOptions = new RetryOptions();
        configure?.Invoke(RetryOptions);
        return this;
    }

    /// <inheritdoc />
    public IRestRequest WithStringContent(string content, Encoding? encoding = null, string? mediaType = null)
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
