// The Sharpener project licenses this file to you under the MIT license.

using FluentAssertions;
using Sharpener.Rest.Extensions;

namespace Sharpener.Rest.Tests.Extensions;

public class HttpExtensionTests
{
    [Fact]
    public void SetBaseAddress_EmptyAddress_ThrowsArgumentNullException()
    {
        var httpClient = new HttpClient();

        var act = () => httpClient.SetBaseAddress(string.Empty);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SetBaseAddress_InvalidUrl_ThrowsArgumentException()
    {
        var httpClient = new HttpClient();

        var act = () => httpClient.SetBaseAddress("invalid-url");

        act.Should().Throw<UriFormatException>();
    }

    [Fact]
    public void SetBaseAddress_ValidAddress_AddsTrailingSlash()
    {
        var httpClient = new HttpClient();

        httpClient.SetBaseAddress("https://api.example.com");

        httpClient.BaseAddress.Should().Be(new Uri("https://api.example.com/"));
    }
}
