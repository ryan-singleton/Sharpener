using FluentAssertions;
using Sharpener.Net.Extensions;
using Sharpener.Net.Preferences;
using System.Text.Json;
using Sharpener.Net.Tests.Models;

public class SerializationExtensionsTests
{
    [Fact]
    public void ToJson_Success()
    {
        var item = new Item("guy", "person");
        var asJson = item.ToJson();
        var compareJson = JsonSerializer.Serialize(item);

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
        SerializationSettings.SetDefaultToJson(_ => "stuff");

        item.ToJson().Should().Be("stuff");

        SerializationSettings.Reset();
    }

    [Fact]
    public void ToJson_SetNamed_Success()
    {
        var item = new Item("guy", "person");
        SerializationSettings.SetNamedToJson("test", _ => "stuff");

        item.ToJson("test").Should().Be("stuff");

        SerializationSettings.Reset();
    }

    [Fact]
    public void FromJson_SetDefault_Success()
    {
        var item = new Item("guy", "person");
        SerializationSettings.SetDefaultFromJson((_, _) => new Item("other", "person"));

        item.ToJson().FromJson<Item>()!.Name.Should().Be("other");

        SerializationSettings.Reset();
    }

    [Fact]
    public void FromJson_SetNamed_Success()
    {
        var item = new Item("guy", "person");
        SerializationSettings.SetNamedFromJson("test", (_, _) => new Item("other", "person"));

        item.ToJson().FromJson<Item>("test")!.Name.Should().Be("other");

        SerializationSettings.Reset();
    }
}