// The Sharpener project licenses this file to you under the MIT license.

using FluentAssertions;
using Sharpener.Json.Extensions;
using Sharpener.Rest.Extensions;
using Sharpener.Rest.Tests.Models;

namespace Sharpener.Rest.Tests.Extensions;

public class DictionaryExtensionTests
{
    [Fact]
    public void ToParameters_Success()
    {
        var item = new Item("guy", "person");
        var parameters = item.ToParameters<string, object>();
        parameters!.WriteJson().Should().Be(item.WriteJson());
    }
}
