// The Sharpener project licenses this file to you under the MIT license.

using System.Collections.Concurrent;

namespace Sharpener.Rest.Factories;

/// <summary>
///     A default client factory when one is not available from ASPNET Core or otherwise. Simply provides a
///     <see cref="ConcurrentDictionary{TKey,TValue}" /> of <see cref="HttpClient" /> and gets or adds to it. Nothing more
///     complex than that.
/// </summary>
public sealed class SharpenerHttpClientFactory : IHttpClientFactory
{
    internal static readonly ConcurrentDictionary<string, HttpClient> CachedClients = new();

    /// <inheritdoc />
    public HttpClient CreateClient(string name)
    {
        if (CachedClients.TryGetValue(name, out var client))
        {
            return client;
        }

        var newClient = new HttpClient();
        CachedClients.TryAdd(name, newClient);
        return newClient;
    }
}
