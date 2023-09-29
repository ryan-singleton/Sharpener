// The Sharpener project licenses this file to you under the MIT license.

using System.Net;
using FluentAssertions;
using Sharpener.Rest.Extensions;
using Sharpener.Rest.Testing;

namespace Sharpener.Rest.Tests;

public class RestRequestTests
{
    private const string FakeBaseUrl = "https://google.com/api/sample/";

    [Fact]
    public async void Get_Story_Success()
    {
        const string fake = "success";
        using var httpTest = new HttpTest(FakeBaseUrl);
        httpTest.RespondWithJson(fake);

        const string name = "someName";
        var restRequest = (RestRequest)httpTest.Client.Rest();
        var result = await restRequest
            .SetPaths("name", "value", "context")
            .SetBearerToken("testToken")
            .AddQueries(new
            {
                id = "someId",
                name
            }).SetJsonContent(new
            {
                id = "also an id",
                name
            }).PostAsync()
            .ReadJsonAs<string>()
            .ConfigureAwait(false);

        restRequest.UriBuilder.Uri.AbsoluteUri.Should()
            .Be("https://google.com/api/sample/name/value/context?id=someId&name=someName");
        result.Value.Should().Be(fake);
        restRequest.Request.Headers.Authorization?.Parameter.Should().Be("testToken");
    }

    [Fact]
    public async Task Retry_ResultsIn200()
    {
        var httpTest = new HttpTest();
        httpTest.RespondWith(new HttpResponseMessage(HttpStatusCode.RequestTimeout),
            new HttpResponseMessage(HttpStatusCode.RequestTimeout),
            new HttpResponseMessage(HttpStatusCode.OK));

        var response = await httpTest.Client.Rest(new Uri("http://localhost"))
            .UseRetry()
            .GetAsync()
            .ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
