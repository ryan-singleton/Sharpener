// The Sharpener project licenses this file to you under the MIT license.

using System.Text;
using Sharpener.Json.Extensions;
using Sharpener.Options;

namespace Sharpener.Rest.Extensions;

/// <summary>
///     Extensions for http json.
/// </summary>
public static class JsonExtensions
{
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
    ///     Packages the provided object as JSON StringContent to be sent over an HttpRequestMessage as content.
    /// </summary>
    /// <param name="source">The object to send as StringContent in the request</param>
    /// <returns>The provided object as <see cref="StringContent" />.</returns>
    public static StringContent ToJsonStringContent(this object source)
    {
        return new StringContent(source.WriteJson(), Encoding.UTF8, "application/json");
    }
}
