// The Sharpener project licenses this file to you under the MIT license.

using System.Net;
using Sharpener.Json.Extensions;
using Sharpener.Rest.Extensions;
using Sharpener.Rest.Tests.Models;

namespace Sharpener.Rest.Tests.Extensions;

public class JsonExtensionTests
{
    [Fact]
    public async Task ReadContentJsonAs_Success()
    {
        var item = new Item("guy", "person");
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = new StringContent(item.WriteJson())
        };

        (await httpResponseMessage.ReadContentJsonAs<Item>())!.Name.ShouldBe(item.Name);
    }

    [Fact]
    public async Task ReadJsonAs_Success()
    {
        var item = new Item("guy", "person");
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = new StringContent(item.WriteJson())
        };

        var task = Task.FromResult(httpResponseMessage);

        var result = await task.ReadJsonAs<Item>();
        result.IsSuccess.ShouldBe(true);
        result.Value!.Name.ShouldBe(item.Name);
    }
}
