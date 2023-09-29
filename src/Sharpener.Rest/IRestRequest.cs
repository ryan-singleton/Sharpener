// The Sharpener project licenses this file to you under the MIT license.

using System.Text;
using Sharpener.Rest.Retry;

namespace Sharpener.Rest;

/// <summary>
///     A request type that can be built with fluent syntax.
/// </summary>
public interface IRestRequest
{
    /// <summary>
    ///     Gets the current <see cref="Uri" /> of the request.
    /// </summary>
    Uri CurrentUri { get; }

    /// <summary>
    ///     Adds the query parameters based upon an object and its properties. This is done with serialization and then
    ///     deserialization to a dictionary.
    /// </summary>
    /// <param name="values">The object defining the query parameters.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest AddQueries(object values);

    /// <summary>
    ///     Adds the query parameter based on name and value. If either value is null, the query parameter is not added. Empty
    ///     strings can bypass this if desired.
    /// </summary>
    /// <param name="name">The name of the query parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest AddQuery(string name, object? value);

    /// <summary>
    ///     Sends the request with a DELETE method.
    /// </summary>
    /// <returns> The <see cref="HttpResponseMessage" /> from the request.</returns>
    Task<HttpResponseMessage> DeleteAsync();

    /// <summary>
    ///     Sends the request with a GET method.
    /// </summary>
    /// <returns> The <see cref="HttpResponseMessage" /> from the request.</returns>
    Task<HttpResponseMessage> GetAsync();

    /// <summary>
    ///     Sends the request with a PATCH method.
    /// </summary>
    /// <returns> The <see cref="HttpResponseMessage" /> from the request.</returns>
    Task<HttpResponseMessage> PatchAsync();

    /// <summary>
    ///     Sends the request with a POST method.
    /// </summary>
    /// <returns> The <see cref="HttpResponseMessage" /> from the request.</returns>
    Task<HttpResponseMessage> PostAsync();

    /// <summary>
    ///     Sends the request with a PUT method.
    /// </summary>
    /// <returns> The <see cref="HttpResponseMessage" /> from the request.</returns>
    Task<HttpResponseMessage> PutAsync();

    /// <summary>
    ///     The logic that sends the request and provides a <see cref="HttpResponseMessage" />.
    /// </summary>
    /// <remarks>
    ///     Are you guys silly? I'm just gonna send it.
    /// </remarks>
    /// <param name="httpMethod">The http method of the request.</param>
    /// <returns> The <see cref="HttpResponseMessage" /> from the request.</returns>
    Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod);

    /// <summary>
    ///     Adds a basic token to the Authentication header.
    /// </summary>
    /// <param name="username">The username of the token.</param>
    /// <param name="password">The password of the token.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetBasicToken(string username, string password);

    /// <summary>
    ///     Adds a Bearer token to the Authentication header.
    /// </summary>
    /// <param name="token">The bearer token.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetBearerToken(string token);

    /// <summary>
    ///     Adds form URL encoded content to the request.
    /// </summary>
    /// <param name="values"> The object defining the form content.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetFormContent(object values);

    /// <summary>
    ///     Adds a custom header to the request.
    /// </summary>
    /// <param name="key">The header key.</param>
    /// <param name="value">The value of the header.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetHeader(string key, string value);

    /// <summary>
    ///     Adds the headers based upon an object and its properties. This is done with serialization and then
    ///     deserialization to a dictionary.
    /// </summary>
    /// <param name="values"> The object defining the headers.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetHeaders(object values);

    /// <summary>
    ///     Adds the provided object as a JSON payload in the request.
    /// </summary>
    /// <param name="content">The request body.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetJsonContent(object? content);

    /// <summary>
    ///     Adds the provided object as a multipart form data payload in the request.
    /// </summary>
    /// <param name="values"> The object defining the form content.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetMultipartFormData(object values);

    /// <summary>
    ///     Adds multipart form data content to the request.
    /// </summary>
    /// <param name="buildContent"> The action that builds the content.</param>
    /// <param name="mediaType"> The media type of the content. Defaults to multipart/form-data.</param>
    /// <param name="boundary"> The boundary of the content. Defaults to ----WebKitFormBoundary7GuI94hQ253xT0v.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetMultipartFormData(Action<MultipartFormDataContent> buildContent,
        string? mediaType = "multipart/form-data", string boundary = "----WebKitFormBoundary7GuI94hQ253xT0v");

    /// <summary>
    ///     Adds values to the request path. They are concatenated with slashes. This will not overwrite previous calls to this
    ///     method.
    /// </summary>
    /// <param name="paths">The path values to add to the request url in the end.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest SetPaths(params string?[] paths);

    /// <summary>
    ///     Uses retry options for the request.
    /// </summary>
    /// <param name="configure">The retry configuration action.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest UseRetry(Action<RetryOptions>? configure = null);

    /// <summary>
    ///     Adds the provided object as a string payload in the request.
    /// </summary>
    /// <param name="content">The request body.</param>
    /// <param name="encoding"> The encoding of the content. Defaults to UTF-8.</param>
    /// <param name="mediaType"> The media type of the content. Defaults to text/plain.</param>
    /// <returns> The <see cref="IRestRequest" /> that is being configured.</returns>
    IRestRequest WithStringContent(string content, Encoding? encoding = null, string? mediaType = null);
}
