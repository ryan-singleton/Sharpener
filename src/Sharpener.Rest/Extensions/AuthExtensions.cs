// The Sharpener project licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Sharpener.Extensions;

namespace Sharpener.Rest.Extensions;

/// <summary>
///     Extensions for authentication and authorization.
/// </summary>
public static class AuthExtensions
{
    /// <summary>
    ///     The name of the authorization header.
    /// </summary>
    internal const string AuthHeader = "Authorization";

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

        if (string.IsNullOrWhiteSpace(authHeader) || authHeader.NoCase().Contains(scheme) != true)
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
