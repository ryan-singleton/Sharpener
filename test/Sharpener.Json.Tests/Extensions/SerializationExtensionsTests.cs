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
        string asJson = item.ToJson();
        string compareJson = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });

        _ = asJson.Should().Be(compareJson);
    }

    [Fact]
    public void FromJson_Success()
    {
        var item = new Item("guy", "person");
        string asJson = item.ToJson();
        Item? asItem = asJson.FromJson<Item>();

        _ = asItem.Should().NotBeNull();
        _ = asItem!.Name.Should().Be(item.Name);
    }

    [Fact]
    public void ToJson_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultSerializer<MockTo>();

        _ = item.ToJson().Should().Be("stuff");

        SharpenerJsonSettings.ResetDefaults();
    }

    [Fact]
    public void ToJson_UseType_Success()
    {
        var item = new Item("guy", "person");

        _ = item.ToJson<MockTo>().Should().Be("stuff");
    }

    [Fact]
    public void FromJson_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultDeserializer<MockFrom>();

        _ = item.ToJson().FromJson<Item>()!.Name.Should().Be("other");

        SharpenerJsonSettings.ResetDefaults();
    }

    [Fact]
    public void FromJson_UseType_Success()
    {
        var item = new Item("guy", "person");

        _ = item.ToJson().FromJson<Item, MockFrom>()!.Name.Should().Be("other");
    }
}
