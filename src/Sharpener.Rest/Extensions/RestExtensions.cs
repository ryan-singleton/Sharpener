// The Sharpener project licenses this file to you under the MIT license.

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Sharpener.Extensions;
using Sharpener.Json.Extensions;
using Sharpener.Options;
using Sharpener.Rest.Retry;

namespace Sharpener.Rest.Extensions;

/// <summary>
///     Extension methods for REST requests.
/// </summary>
public static class RestExtensions
{
    /// <summary>
    ///     The name of the authorization header.
    /// </summary>
    internal const string AuthHeader = "Authorization";

    /// <summary>
    ///     The default retry status codes.
    /// </summary>
    public static readonly HttpStatusCode[] DefaultRetryStatusCodes =
    {
        HttpStatusCode.RequestTimeout, (HttpStatusCode)425, (HttpStatusCode)429, HttpStatusCode.InternalServerError,
        HttpStatusCode.BadGateway, HttpStatusCode.ServiceUnavailable, HttpStatusCode.GatewayTimeout
    };

    /// <summary>
    ///     Creates an <see cref="HttpClient" /> that is named by the url that is also the assigned base address.
    /// </summary>
    /// <param name="factory"> The factory that will create the client.</param>
    /// <param name="baseUrl"> The base url that will be assigned to the <see cref="HttpClient.BaseAddress" />.</param>
    /// <returns> The <see cref="HttpClient" /> with a base address and an associated class name for which it serves.</returns>
    public static HttpClient CreateUrlClient(this IHttpClientFactory factory, string baseUrl)
    {
        var client = factory.CreateClient(baseUrl.GetHashCode().ToString(CultureInfo.CurrentCulture));
        client.SetBaseAddress(baseUrl);
        return client;
    }

    /// <summary>
    ///     Creates an <see cref="HttpClient" /> that is named by the class type that it serves for later factory calls.
    /// </summary>
    /// <param name="httpClientFactory">The factory that will create the client.</param>
    /// <param name="baseUrl">The base url that will be assigned to the <see cref="HttpClient.BaseAddress" />.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The <see cref="HttpClient" /> with a base address and an associated class name for which it serves.</returns>
    public static HttpClient CreateTypeClient<T>(this IHttpClientFactory httpClientFactory, string? baseUrl = null)
    {
        var client = httpClientFactory.CreateClient(typeof(T).Name);
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            client.BaseAddress = new Uri(baseUrl);
        }

        return client;
    }

    /// <summary>
    ///     Retrieves the auth token with the specified scheme from the headers dictionary.
    /// </summary>
    /// <param name="headers">The headers dictionary.</param>
    /// <param name="scheme">The auth scheme.</param>
    /// <returns>The auth token if found; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when headers or scheme is null or empty.</exception>
    public static string? GetAuthToken(this HttpHeaders headers, string scheme)
    {
        return headers.GetAuthenticationTokenInternal(scheme);
    }

    /// <inheritdoc cref="GetAuthToken(HttpHeaders, string)" />
    public static string? GetAuthToken(this IHeaderDictionary headers, string scheme)
    {
        return headers.GetAuthenticationTokenInternal(scheme);
    }

    /// <summary>
    ///     Retrieves the basic token from the headers dictionary.
    /// </summary>
    /// <param name="headers">The headers dictionary.</param>
    /// <returns>The basic token if found; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when headers is null.</exception>
    public static string? GetBasicToken(this HttpHeaders headers)
    {
        return headers.GetAuthToken("Basic");
    }

    /// <inheritdoc cref="GetBasicToken(HttpHeaders)" />
    public static string? GetBasicToken(this IHeaderDictionary headers)
    {
        return headers.GetAuthToken("Basic");
    }

    /// <summary>
    ///     Retrieves the bearer token from the headers dictionary.
    /// </summary>
    /// <param name="headers">The headers dictionary.</param>
    /// <returns>The bearer token if found; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when headers is null.</exception>
    public static string? GetBearerToken(this HttpHeaders headers)
    {
        return headers.GetAuthToken("Bearer");
    }

    /// <inheritdoc cref="GetBearerToken(HttpHeaders)" />
    public static string? GetBearerToken(this IHeaderDictionary headers)
    {
        return headers.GetAuthToken("Bearer");
    }

    /// <summary>
    ///     Helper method to retrieve form data as a dictionary
    /// </summary>
    /// <param name="content">The convent to convert.</param>
    /// <returns>The form content as a dictionary.</returns>
    public static async Task<Dictionary<string, string>> GetFormDataAsync(this HttpContent? content)
    {
        if (content is null)
        {
            return new Dictionary<string, string>();
        }

        var formData = await content.ReadAsByteArrayAsync().ConfigureAwait(false);
        var formDataString = Encoding.UTF8.GetString(formData);
        var formDataPairs = formDataString.Split('&');

        var dictionary = new Dictionary<string, string>();
        foreach (var pair in formDataPairs)
        {
            var keyValue = pair.Split('=');
            var key = Uri.UnescapeDataString(keyValue[0]);
            var value = Uri.UnescapeDataString(keyValue[1]);
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    /// <summary>
    ///     Determines if the response status code indicates that the request should be retried.
    /// </summary>
    /// <remarks>
    ///     If not provided, the default retry codes are: 408, 425, 429, 500, 502, 503, 504.
    /// </remarks>
    /// <param name="httpResponseMessage">
    ///     The <see cref="HttpResponseMessage" /> to check.
    /// </param>
    /// <param name="retryCodes">
    ///     The HTTP status codes that will trigger a retry.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the response status code indicates that the request should be retried; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsRetryStatusCode(this HttpResponseMessage httpResponseMessage,
        params HttpStatusCode[]? retryCodes)
    {
        retryCodes = retryCodes.IsNullOrEmpty() ? DefaultRetryStatusCodes : retryCodes;
        return retryCodes.Contains(httpResponseMessage.StatusCode);
    }

    /// <summary>
    ///     Helper method to read the response to an <see cref="HttpResponseMessage" /> deserialized to a type.
    /// </summary>
    /// <param name="httpResponseMessage">The http response message whose content is to be deserialized.</param>
    /// <param name="template">The object that will serve as a template for deserialization, best used with anonymous types.</param>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <returns>The deserialized content as the response type.</returns>
    public static async Task<T?> ReadContentJsonAs<T>(this HttpResponseMessage httpResponseMessage,
        T? template = default)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        return content.ReadJsonAs<T>();
    }

    /// <summary>
    ///     Returns the results of the request as a <see cref="Option{T,TAlt}" /> with the deserialized result when
    ///     successful, or an <see cref="HttpResponseMessage" /> when it fails.
    /// </summary>
    /// <param name="task">Tha task that will return the <see cref="HttpResponseMessage" />.</param>
    /// <param name="readJson">The optional JSON deserialization override.</param>
    /// <returns>
    ///     A <see cref="Option{T,TAlt}" /> containing either the deserialized response, or the
    ///     <see cref="HttpResponseMessage" /> if it was a failure.
    /// </returns>
    public static async Task<Option<T, HttpResponseMessage>> ReadJsonAs<T>(this Task<HttpResponseMessage> task,
        Func<HttpResponseMessage, Task<T?>>? readJson = null)
    {
        var response = await task.ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return response;
        }

        readJson ??= message => message.ReadContentJsonAs<T>();
        return (await readJson(response).ConfigureAwait(false), response);
    }

    /// <summary>
    ///     Creates a <see cref="RestRequest" /> using the <see cref="HttpClient" />. The client must have a base address
    ///     assigned.
    /// </summary>
    /// <param name="httpClient">
    ///     The http client to eventually send the fluently built request with. Must have a base address
    ///     assigned.
    /// </param>
    /// <param name="baseAddress">
    ///     The base address to assign if one is not already set. If null, the <see cref="HttpClient" />
    ///     's current base address will be unaffected.
    /// </param>
    /// <returns></returns>
    public static RestRequest Rest(this HttpClient httpClient, Uri? baseAddress = null)
    {
        if (baseAddress is not null)
        {
            httpClient.BaseAddress = baseAddress;
        }

        return new RestRequest(httpClient);
    }

    /// <summary>
    ///     Sets an auth token with the specified scheme to the headers dictionary.
    /// </summary>
    /// <param name="headers">The headers dictionary.</param>
    /// <param name="scheme">The auth scheme.</param>
    /// <param name="token">The auth token.</param>
    /// <exception cref="ArgumentNullException">Thrown when headers, scheme, or token is null or empty.</exception>
    public static void SetAuthToken(this HttpHeaders headers, string scheme,
        string token)
    {
        headers.SetAuthenticationTokenInternal(scheme, token);
    }

    /// <inheritdoc cref="SetAuthToken(HttpHeaders, string, string)" />
    public static void SetAuthToken(this IHeaderDictionary headers, string scheme,
        string token)
    {
        headers.SetAuthenticationTokenInternal(scheme, token);
    }

    /// <summary>
    ///     Sets the base address of the <see cref="HttpClient" /> instance, ensuring it ends with a "/".
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient" /> instance.</param>
    /// <param name="baseAddress">The base address to set.</param>
    public static void SetBaseAddress(this HttpClient httpClient, string baseAddress)
    {
        if (string.IsNullOrWhiteSpace(baseAddress))
        {
            throw new ArgumentNullException(nameof(baseAddress));
        }

        if (!baseAddress.EndsWith("/"))
        {
            baseAddress += "/";
        }

        httpClient.BaseAddress = new Uri(baseAddress);
    }

    /// <summary>
    ///     Sets a basic token to the headers dictionary.
    /// </summary>
    /// <param name="headers">The headers dictionary.</param>
    /// <param name="username">The username for the basic token.</param>
    /// <param name="password">The password for the basic token.</param>
    /// <exception cref="ArgumentNullException">Thrown when headers, username, or password is null or empty.</exception>
    public static void SetBasicToken(this HttpHeaders headers, string username,
        string password)
    {
        var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        headers.SetAuthToken("Basic", token);
    }

    /// <inheritdoc cref="SetBasicToken(HttpHeaders,string,string)" />
    public static void SetBasicToken(this IHeaderDictionary headers, string username,
        string password)
    {
        var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        headers.SetAuthToken("Basic", token);
    }

    /// <summary>
    ///     Sets a bearer token to the headers dictionary.
    /// </summary>
    /// <param name="headers">The headers dictionary.</param>
    /// <param name="token">The bearer token.</param>
    /// <exception cref="ArgumentNullException">Thrown when headers or token is null or empty.</exception>
    public static void SetBearerToken(this HttpHeaders headers, string token)
    {
        headers.SetAuthToken("Bearer", token);
    }

    /// <inheritdoc cref="SetBearerToken(HttpHeaders,string)" />
    public static void SetBearerToken(this IHeaderDictionary headers, string token)
    {
        headers.SetAuthToken("Bearer", token);
    }

    /// <summary>
    ///     Packages the provided object as JSON StringContent to be sent over an HttpRequestMessage as content.
    /// </summary>
    /// <param name="source">The object to send as StringContent in the request</param>
    /// <returns>The provided object as <see cref="StringContent" />.</returns>
    public static StringContent ToJsonStringContent(this object source)
    {
        return new StringContent(source.WriteJson(), Encoding.UTF8, "application/json");
    }

    /// <summary>
    ///     Converts an anonymous entity to a <see cref="Dictionary{TKey,TValue}" /> by serializing, then deserializing back
    ///     into a dictionary.
    /// </summary>
    /// <param name="entity">The entity to convert into a dictionary.</param>
    /// <returns>A dictionary of properties made from the object.</returns>
    public static Dictionary<TKey, TValue?>? ToParameters<TKey, TValue>(this object entity) where TKey : notnull
    {
        var json = entity.WriteJson();
        return json.ReadJsonAs<Dictionary<TKey, TValue?>>();
    }

    /// <summary>
    ///     Retries an HTTP request based on the provided options.
    /// </summary>
    /// <param name="func">The function to retry.</param>
    /// <param name="configure">The configuration of the retry options.</param>
    /// <returns>A response when the task eventually succeeds or exceeds the max retry.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<HttpResponseMessage> WithRetry(this Func<Task<HttpResponseMessage>> func,
        Action<RetryOptions>? configure = null)
    {
        var options = new RetryOptions();
        configure?.Invoke(options);

        return await func.WithRetry(options).ConfigureAwait(false);
    }

    /// <summary>
    ///     Retries an HTTP request based on the provided options.
    /// </summary>
    /// <param name="func">The function to retry.</param>
    /// <param name="options">The options for the retries.</param>
    /// <returns>A response when the task eventually succeeds or exceeds the max retry.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<HttpResponseMessage> WithRetry(this Func<Task<HttpResponseMessage>> func,
        RetryOptions options)
    {
        var attempts = 0;
        var response = await func().ConfigureAwait(false);

        while (attempts < options.MaximumAttempts && options.Requirement is not null &&
               !await options.Requirement(response).ConfigureAwait(false))
        {
            attempts++;
            options.Acknowledgement?.Invoke(attempts, response.StatusCode);
            await Task.Delay(options.Delay).ConfigureAwait(false);

            options.UpdateBackoff();
            response = await func().ConfigureAwait(false);
        }

        return response;
    }

    /// <summary>
    ///     Retrieves the auth token with the specified scheme from the headers dictionary.
    /// </summary>
    private static string? GetAuthenticationTokenInternal(this object headers, string scheme)
    {
        if (headers == null)
        {
            throw new ArgumentNullException(nameof(headers));
        }

        if (string.IsNullOrWhiteSpace(scheme))
        {
            throw new ArgumentNullException(nameof(scheme));
        }

        string? authHeader = null;
        switch (headers)
        {
            case IHeaderDictionary headerDictionary:
            {
                if (headerDictionary.TryGetValue(AuthHeader, out var authValues))
                {
                    authHeader = authValues.FirstOrDefault();
                }

                break;
            }
            case HttpHeaders httpHeaders:
            {
                if (httpHeaders.TryGetValues(AuthHeader, out var authValues))
                {
                    authHeader = authValues.FirstOrDefault();
                }

                break;
            }
        }

        if (string.IsNullOrWhiteSpace(authHeader) || authHeader?.NoCase().Contains(scheme) != true)
        {
            return null;
        }

        var token = authHeader.Replace($"{scheme} ", "");
        return token;
    }

    /// <summary>
    ///     Sets an auth token with the specified scheme to the headers dictionary.
    /// </summary>
    private static void SetAuthenticationTokenInternal(this object headers, string scheme, string token)
    {
        if (headers == null)
        {
            throw new ArgumentNullException(nameof(headers));
        }

        if (string.IsNullOrWhiteSpace(scheme))
        {
            throw new ArgumentNullException(nameof(scheme));
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentNullException(nameof(token));
        }

        switch (headers)
        {
            case IHeaderDictionary headerDictionary:
                headerDictionary[AuthHeader] = $"{scheme} {token}";
                break;
            case HttpHeaders httpHeaders:
                if (httpHeaders.Contains(AuthHeader))
                {
                    httpHeaders.Remove(AuthHeader);
                }

                httpHeaders.Add(AuthHeader, $"{scheme} {token}");
                break;
        }
    }
}
