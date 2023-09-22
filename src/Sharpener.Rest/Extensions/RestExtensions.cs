// The Sharpener project licenses this file to you under the MIT license.

using System.Globalization;
using System.Web;
using Sharpener.Json.Extensions;

namespace Sharpener.Rest.Extensions;

/// <summary>
///     Extension methods for REST requests.
/// </summary>
public static class RestExtensions
{
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
    ///     Generates a url encoded string from an object.
    /// </summary>
    /// <param name="data">The object to convert to a url encoded string.</param>
    /// <returns>The url encoded string.</returns>
    public static string ToUrlEncodedString(this object data)
    {
        var properties = from propertyInfo in data.GetType().GetProperties()
            where propertyInfo.GetValue(data, null) is not null
            select $"{propertyInfo.Name}={HttpUtility.UrlEncode(propertyInfo.GetValue(data, null).ToString())}";

        return string.Join("&", properties.ToArray());
    }
}
