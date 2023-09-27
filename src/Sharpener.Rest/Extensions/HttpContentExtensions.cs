// The Sharpener project licenses this file to you under the MIT license.

using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using Sharpener.Json.Extensions;

namespace Sharpener.Rest.Extensions;

/// <summary>
///     Extensions for http content.
/// </summary>
public static class HttpContentExtensions
{
    /// <summary>
    ///     Adds a multipart form data part to the <see cref="MultipartFormDataContent" />.
    /// </summary>
    /// <param name="content">The <see cref="MultipartFormDataContent" /> to add the part to.</param>
    /// <param name="name">The control name of the part.</param>
    /// <param name="httpContent">The content of the part.</param>
    /// <exception cref="ArgumentException">name is required</exception>
    public static void AddContent(this MultipartFormDataContent content, string name, HttpContent httpContent)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        }

        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = name };
        content.Add(httpContent);
    }

    /// <summary>
    ///     Adds a file to the <see cref="MultipartFormDataContent" /> from a local path.
    /// </summary>
    /// <param name="content"> The <see cref="MultipartFormDataContent" /> to add the file to.</param>
    /// <param name="name">The control name of the part.</param>
    /// <param name="file">The local path to the file.</param>
    /// <param name="mediaType"> The media type of the file. Defaults to application/octet-stream.</param>
    /// <exception cref="ArgumentException">name must not be empty</exception>
    /// <exception cref="ArgumentException">file must not be empty</exception>
    public static void AddFile(this MultipartFormDataContent content, string name, string file,
        string? mediaType = "application/octet-stream")
    {
        if (string.IsNullOrWhiteSpace(file))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(file));
        }

        if (!File.Exists(file))
        {
            throw new FileNotFoundException("File not found.", file);
        }

        var filename = Path.GetFileName(file);
        var fileStream = File.OpenRead(file);
        var fileContent = new StreamContent(fileStream);
        if (!string.IsNullOrWhiteSpace(mediaType))
        {
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        }

        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = name,
            FileName = filename,
            FileNameStar = filename
        };

        content.Add(fileContent, name, Path.GetFileName(file));
    }

    /// <summary>
    ///     Adds a json part to the <see cref="MultipartFormDataContent" />.
    /// </summary>
    /// <param name="content">The <see cref="MultipartFormDataContent" /> to add the part to.</param>
    /// <param name="name">The control name of the part.</param>
    /// <param name="data">The content of the part, which will be serialized to JSON.</param>
    /// <param name="mediaType"> The media type of the json. Defaults to application/json.</param>
    /// <exception cref="ArgumentException">name must not be empty</exception>
    public static void AddJson(this MultipartFormDataContent content, string name, object data,
        string? mediaType = "application/json")
    {
        content.AddContent(name, new StringContent(data.WriteJson(), Encoding.UTF8, mediaType));
    }

    /// <summary>
    ///     Adds a simple string part to the <see cref="MultipartFormDataContent" />.
    /// </summary>
    /// <param name="content">The <see cref="MultipartFormDataContent" /> to add the part to.</param>
    /// <param name="name">The control name of the part.</param>
    /// <param name="value">The string value of the part.</param>
    /// <param name="mediaType"> The media type of the part. Defaults to text/plain.</param>
    /// <exception cref="ArgumentException">name must not be empty</exception>
    public static void AddString(this MultipartFormDataContent content, string name, string value,
        string? mediaType = "text/plain")
    {
        content.AddContent(name, new StringContent(value, Encoding.UTF8, mediaType));
    }

    /// <summary>
    ///     Adds a set of string parts to the <see cref="MultipartFormDataContent" />.
    /// </summary>
    /// <param name="content">The <see cref="MultipartFormDataContent" /> to add the parts to.</param>
    /// <param name="data">The data to add.</param>
    /// <param name="mediaType"> The media type of the parts. Defaults to text/plain.</param>
    /// <exception cref="ArgumentException">Data failed to convert to key value pairs.</exception>
    public static void AddStringParts(this MultipartFormDataContent content, object data,
        string? mediaType = "text/plain")
    {
        var parameters = data.ToParameters<string, string>();
        if (parameters is null || parameters.Count == 0)
        {
            throw new ArgumentException("Failed to get key value pairs from post data.", nameof(data));
        }

        foreach (var pair in parameters.Where(pair =>
                     !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value)))
        {
            content.AddString(pair.Key, pair.Value?.ToString(CultureInfo.InvariantCulture)!, mediaType);
        }
    }

    /// <summary>
    ///     Adds a url encoded part to the <see cref="MultipartFormDataContent" />.
    /// </summary>
    /// <param name="content">The <see cref="MultipartFormDataContent" /> to add the part to.</param>
    /// <param name="name">The control name of the part.</param>
    /// <param name="data">The content of the part, whose properties will be parsed and serialized to URL-encoded format.</param>
    /// <param name="mediaType"> The media type of the form. Defaults to application/x-www-form-urlencoded.</param>
    /// <exception cref="ArgumentException">name must not be empty</exception>
    public static void AddUrlEncoded(this MultipartFormDataContent content, string name, object data,
        string? mediaType = "application/x-www-form-urlencoded")
    {
        content.AddContent(name,
            new StringContent(data.ToUrlEncodedString(), Encoding.UTF8, mediaType));
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
}
