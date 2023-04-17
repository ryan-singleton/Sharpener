// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Json.Tests.Extensions;

using System.Text.Json;
using Json.Extensions;
using Mocks;
using Sharpener.Tests.Common.Models;
using Types;

public class SerializationExtensionsTests
{
    [Fact]
    public void ReadJsonAs_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultReader<JsonMockReader>();
        item.WriteJson().ReadJsonAs<Item>()!.Name.Should().Be("other");
        SharpenerJsonSettings.ResetDefaults();
    }

    [Fact]
    public void ReadJsonAs_SetDefaultFunc_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultReader((_, _) => new Item("other", "person"));
        item.WriteJson().ReadJsonAs<Item>()!.Name.Should().Be("other");
        SharpenerJsonSettings.ResetDefaults();
    }

    [Fact]
    public void ReadJsonAs_Success()
    {
        var item = new Item("guy", "person");
        var asJson = item.WriteJson();
        var asItem = asJson.ReadJsonAs<Item>();
        asItem.Should().NotBeNull();
        asItem!.Name.Should().Be(item.Name);
    }

    [Fact]
    public void ReadJsonAs_UseType_Success()
    {
        var item = new Item("guy", "person");
        item.WriteJson().ReadJsonAs<Item, JsonMockReader>()!.Name.Should().Be("other");
    }

    [Fact]
    public void WriteJson_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultWriter<JsonMockWriter>();
        item.WriteJson().Should().Be("stuff");
        SharpenerJsonSettings.ResetDefaults();
    }

    [Fact]
    public void WriteJson_SetDefaultFunc_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultWriter(_ => "stuff");
        item.WriteJson().Should().Be("stuff");
        SharpenerJsonSettings.ResetDefaults();
    }

    [Fact]
    public void WriteJson_Success()
    {
        var item = new Item("guy", "person");
        var asJson = item.WriteJson();
        var compareJson = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
        asJson.Should().Be(compareJson);
    }

    [Fact]
    public void WriteJson_UseType_Success()
    {
        var item = new Item("guy", "person");
        item.WriteJson<JsonMockWriter>().Should().Be("stuff");
    }
}
