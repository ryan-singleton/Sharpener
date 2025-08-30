// The Sharpener project licenses this file to you under the MIT license.


using Sharpener.Rest.Extensions;

namespace Sharpener.Rest.Tests.Extensions;

public class HttpExtensionTests
{
    [Fact]
    public void SetBaseAddress_EmptyAddress_ThrowsArgumentNullException()
    {
        var httpClient = new HttpClient();

        var act = () => httpClient.SetBaseAddress(string.Empty);
        act.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void SetBaseAddress_InvalidUrl_ThrowsArgumentException()
    {
        var httpClient = new HttpClient();

        var act = () => httpClient.SetBaseAddress("invalid-url");

        act.ShouldThrow<UriFormatException>();
    }

    [Fact]
    public void SetBaseAddress_ValidAddress_AddsTrailingSlash()
    {
        var httpClient = new HttpClient();

        httpClient.SetBaseAddress("https://api.example.com");

        httpClient.BaseAddress.ShouldBe(new Uri("https://api.example.com/"));
    }
}
