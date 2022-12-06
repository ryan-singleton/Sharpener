// The Sharpener project and Facefire license this file to you under the MIT license.

using System.Text.Json;
using Sharpener.Json.Extensions;
using Sharpener.Json.Tests.Mocks;
using Sharpener.Json.Types;
using Sharpener.Tests.Common.Models;
public class SerializationExtensionsTests
{
    [Fact]
    public void ToJson_Success()
    {
        var item = new Item("guy", "person");
        var asJson = item.ToJson();
        var compareJson = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
        asJson.Should().Be(compareJson);
    }
    [Fact]
    public void FromJson_Success()
    {
        var item = new Item("guy", "person");
        var asJson = item.ToJson();
        var asItem = asJson.FromJson<Item>();
        asItem.Should().NotBeNull();
        asItem!.Name.Should().Be(item.Name);
    }
    [Fact]
    public void ToJson_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultSerializer<MockTo>();
        item.ToJson().Should().Be("stuff");
        SharpenerJsonSettings.ResetDefaults();
    }
    [Fact]
    public void ToJson_UseType_Success()
    {
        var item = new Item("guy", "person");
        item.ToJson<MockTo>().Should().Be("stuff");
    }
    [Fact]
    public void FromJson_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultDeserializer<MockFrom>();
        item.ToJson().FromJson<Item>()!.Name.Should().Be("other");
        SharpenerJsonSettings.ResetDefaults();
    }
    [Fact]
    public void FromJson_UseType_Success()
    {
        var item = new Item("guy", "person");
        item.ToJson().FromJson<Item, MockFrom>()!.Name.Should().Be("other");
    }
}
