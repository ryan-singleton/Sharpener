// The Sharpener project licenses this file to you under the MIT license.

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Sharpener.Rest.Extensions;

namespace Sharpener.Rest.Tests.Extensions;

public class AuthenticationExtensionTests
{
    [Fact]
    public void GetAuthToken_HeaderDictionary_ShouldReturnNull_WhenSchemeDoesNotExist()
    {
        var headers = new DefaultHttpContext().Request.Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetAuthToken("Basic");

        token.Should().BeNull();
    }

    [Fact]
    public void GetAuthToken_HeaderDictionary_ShouldReturnToken_WhenSchemeExists()
    {
        var headers = new DefaultHttpContext().Request.Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetAuthToken("Bearer");

        token.Should().Be("testToken");
    }

    [Fact]
    public void GetAuthToken_HttpHeaders_ShouldReturnNull_WhenSchemeDoesNotExist()
    {
        var headers = new HttpRequestMessage().Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetAuthToken("Basic");

        token.Should().BeNull();
    }

    [Fact]
    public void GetAuthToken_HttpHeaders_ShouldReturnToken_WhenSchemeExists()
    {
        var headers = new HttpRequestMessage().Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetAuthToken("Bearer");

        token.Should().Be("testToken");
    }

    [Fact]
    public void GetBasicToken_HeaderDictionary_ShouldReturnNull_WhenBasicSchemeDoesNotExist()
    {
        var headers = new DefaultHttpContext().Request.Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetBasicToken();

        token.Should().BeNull();
    }

    [Fact]
    public void GetBasicToken_HeaderDictionary_ShouldReturnToken_WhenBasicSchemeExists()
    {
        var headers = new DefaultHttpContext().Request.Headers;
        headers.Add(RestExtensions.AuthHeader, "Basic dXNlcm5hbWU6cGFzc3dvcmQ=");

        var token = headers.GetBasicToken();

        token.Should().Be("dXNlcm5hbWU6cGFzc3dvcmQ=");
    }

    [Fact]
    public void GetBasicToken_HttpHeaders_ShouldReturnNull_WhenBasicSchemeDoesNotExist()
    {
        var headers = new HttpRequestMessage().Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetBasicToken();

        token.Should().BeNull();
    }

    [Fact]
    public void GetBasicToken_HttpHeaders_ShouldReturnToken_WhenBasicSchemeExists()
    {
        var headers = new HttpRequestMessage().Headers;
        headers.Add(RestExtensions.AuthHeader, "Basic dXNlcm5hbWU6cGFzc3dvcmQ=");

        var token = headers.GetBasicToken();

        token.Should().Be("dXNlcm5hbWU6cGFzc3dvcmQ=");
    }

    [Fact]
    public void GetBearerToken_HeaderDictionary_ShouldReturnNull_WhenBearerSchemeDoesNotExist()
    {
        var headers = new DefaultHttpContext().Request.Headers;
        headers.Add(RestExtensions.AuthHeader, "Basic dXNlcm5hbWU6cGFzc3dvcmQ=");

        var token = headers.GetBearerToken();

        token.Should().BeNull();
    }

    [Fact]
    public void GetBearerToken_HeaderDictionary_ShouldReturnToken_WhenBearerSchemeExists()
    {
        var headers = new DefaultHttpContext().Request.Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetBearerToken();

        token.Should().Be("testToken");
    }

    [Fact]
    public void GetBearerToken_HttpHeaders_ShouldReturnNull_WhenBearerSchemeDoesNotExist()
    {
        var headers = new HttpRequestMessage().Headers;
        headers.Add(RestExtensions.AuthHeader, "Basic dXNlcm5hbWU6cGFzc3dvcmQ=");

        var token = headers.GetBearerToken();

        token.Should().BeNull();
    }

    [Fact]
    public void GetBearerToken_HttpHeaders_ShouldReturnToken_WhenBearerSchemeExists()
    {
        var headers = new HttpRequestMessage().Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer testToken");

        var token = headers.GetBearerToken();

        token.Should().Be("testToken");
    }

    [Fact]
    public void SetAuthToken_HeaderDictionary_ShouldAddTokenToHeaders()
    {
        var headers = new DefaultHttpContext().Request.Headers;

        headers.SetAuthToken("Bearer", "testToken");

        headers.Should().ContainSingle(h =>
            h.Key == RestExtensions.AuthHeader && h.Value.Contains("Bearer testToken"));
    }

    [Fact]
    public void SetAuthToken_HeaderDictionary_ShouldUpdateTokenInHeaders()
    {
        var headers = new DefaultHttpContext().Request.Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer oldToken");

        headers.SetAuthToken("Bearer", "newToken");

        headers.Should().ContainSingle(h =>
            h.Key == RestExtensions.AuthHeader && h.Value.Contains("Bearer newToken"));
    }

    [Fact]
    public void SetAuthToken_HttpHeaders_ShouldAddTokenToHeaders()
    {
        var headers = new HttpRequestMessage().Headers;

        headers.SetAuthToken("Bearer", "testToken");

        headers.Should().ContainSingle(h =>
            h.Key == RestExtensions.AuthHeader && h.Value.Contains("Bearer testToken"));
    }

    [Fact]
    public void SetAuthToken_HttpHeaders_ShouldUpdateTokenInHeaders()
    {
        var headers = new HttpRequestMessage().Headers;
        headers.Add(RestExtensions.AuthHeader, "Bearer oldToken");

        headers.SetAuthToken("Bearer", "newToken");

        headers.Should().ContainSingle(h =>
            h.Key == RestExtensions.AuthHeader && h.Value.Contains("Bearer newToken"));
    }

    [Fact]
    public void SetBasicToken_HeaderDictionary_ShouldAddTokenToHeaders()
    {
        var headers = new DefaultHttpContext().Request.Headers;

        headers.SetBasicToken("username", "password");

        headers.Should()
            .ContainSingle(h => h.Key == RestExtensions.AuthHeader && h.Value[0]!.Contains("Basic "));
        headers.GetBasicToken().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void SetBasicToken_HttpHeaders_ShouldAddTokenToHeaders()
    {
        var headers = new HttpRequestMessage().Headers;

        headers.SetBasicToken("username", "password");

        headers.Should().ContainSingle(h =>
            h.Key == RestExtensions.AuthHeader && h.Value.First().Contains("Basic "));
        headers.GetBasicToken().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void SetBearerToken_HeaderDictionary_ShouldAddTokenToHeaders()
    {
        var headers = new DefaultHttpContext().Request.Headers;

        headers.SetBearerToken("testToken");

        headers.Should().ContainSingle(h =>
            h.Key == RestExtensions.AuthHeader && h.Value.Contains("Bearer testToken"));
    }

    [Fact]
    public void SetBearerToken_HttpHeaders_ShouldAddTokenToHeaders()
    {
        var headers = new HttpRequestMessage().Headers;

        headers.SetBearerToken("testToken");

        headers.Should().ContainSingle(h =>
            h.Key == RestExtensions.AuthHeader && h.Value.Contains("Bearer testToken"));
    }
}
