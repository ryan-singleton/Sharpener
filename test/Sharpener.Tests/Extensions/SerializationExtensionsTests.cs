using System.Text.Json;
using FluentAssertions;
using Sharpener.Extensions;
using Sharpener.Tests.Mocks;
using Sharpener.Tests.Models;
using Sharpener.Types.Serialization;
using Xunit;

public class SerializationExtensionsTests
{
    [Fact]
    public void ToJson_Success()
    {
        var item = new Item("guy", "person");
        var asJson = item.ToJson();
        var compareJson = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });

        _ = asJson.Should().Be(compareJson);
    }

    [Fact]
    public void FromJson_Success()
    {
        var item = new Item("guy", "person");
        var asJson = item.ToJson();
        var asItem = asJson.FromJson<Item>();

        _ = asItem.Should().NotBeNull();
        _ = asItem!.Name.Should().Be(item.Name);
    }

    [Fact]
    public void ToJson_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SharpenerJsonSettings.SetDefaultSerializer<MockTo>();

        _ = item.ToJson().Should().Be("stuff");

        SharpenerJsonSettings.SetDefaults();
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

        SharpenerJsonSettings.SetDefaults();
    }

    [Fact]
    public void FromJson_UseType_Success()
    {
        var item = new Item("guy", "person");

        _ = item.ToJson().FromJson<Item, MockFrom>()!.Name.Should().Be("other");
    }
}
