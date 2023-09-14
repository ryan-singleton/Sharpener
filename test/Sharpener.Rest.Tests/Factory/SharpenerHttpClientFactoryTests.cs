// The Sharpener project licenses this file to you under the MIT license.


using FluentAssertions;
using Sharpener.Rest.Factories;

namespace Sharpener.Rest.Tests.Factory;

public class SharpenerHttpClientFactoryTests
{
    [Fact]
    public void CreateClient_Should_Add_New_Client_To_Cache()
    {
        var factory = new SharpenerHttpClientFactory();
        const string clientName = "TestClient";
        var client = factory.CreateClient(clientName);
        client.Should().NotBeNull();
        SharpenerHttpClientFactory.CachedClients.Should().ContainKey(clientName);
        SharpenerHttpClientFactory.CachedClients[clientName].Should().Be(client);
    }

    [Fact]
    public void CreateClient_Should_Return_Existing_Cached_Client()
    {
        var factory = new SharpenerHttpClientFactory();
        const string clientName = "TestClient";
        var existingClient = new HttpClient();
        SharpenerHttpClientFactory.CachedClients[clientName] = existingClient;
        var client = factory.CreateClient(clientName);
        client.Should().Be(existingClient);
    }
}
