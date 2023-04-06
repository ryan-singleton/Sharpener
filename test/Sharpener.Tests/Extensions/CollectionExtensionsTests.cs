// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Tests.Extensions;

using Common.Models;

public class CollectionExtensionsTests
{
    public CollectionExtensionsTests()
    {
        _items = new[] { new Item("Item", "FirstItem"), new Item("Item", "SecondItem") };
    }

    private readonly IEnumerable<Item> _items;

    [Fact]
    public void Array_Add_Success()
    {
        var names = new[] { "Sherry", "Sam", "Chris" };
        var count = names.Length;
        names = names.Add("Lisa");
        names.Should().HaveCount(count + 1);
    }

    [Fact]
    public void Array_AddRange_Success()
    {
        var names = new[] { "Sherry", "Sam", "Chris" };
        var count = names.Length;
        var newNames = new[] { "Walter", "Teri", "Art" };
        names = names.AddRange(newNames);
        names.Should().HaveCount(count + newNames.Length);
    }

    [Fact]
    public void Array_Remove_Success()
    {
        var names = new[] { "Sherry", "Sam", "Chris" };
        var count = names.Length;
        names = names.Remove("Chris");
        names.Should().HaveCount(count - 1);
    }

    [Fact]
    public void Array_RemoveAll_Success()
    {
        var names = new[] { "Sherry", "Chris", "Sam", "Chris" };
        var count = names.Length;
        names = names.RemoveAll(x => x.Equals("Chris"));
        names.Should().HaveCount(count - 2);
    }

    [Fact]
    public void ForAll_Success()
    {
        const string newName = "AffectedItem";
        _items.ForAll(x => x.Name = newName);
        _items.All(x => x.Name.Equals(newName)).Should().BeTrue();
    }

    [Fact]
    public void LeftJoin_Success()
    {
        var leftItems = new List<Item>
        {
            new("Bob", "Friend"),
            new("Jane", "Friend"),
            new("Seth", "Friend"),
            new("Gary", "Family"),
            new("Shane", "Coworker")
        };
        var rightItems = new List<Item?>
        {
            new("Bob", "Friend"),
            new("Henry", "Friend"),
            new("Will", "Friend"),
            new("Gary", "Family"),
            new("Shane", "Coworker"),
            new("Sally", "Coworker")
        };
        var results = leftItems.LeftJoin(rightItems,
            left => left.Name,
            right => right?.Name,
            (left, right) => new { Left = left, Right = right }).AsList();

        results.Any(x => x.Left.Name.Equals("Bob")).Should().BeTrue();
        results.Any(x => x.Left.Name.Equals("Jane")).Should().BeTrue();
        results.Any(x => x.Right is null).Should().BeTrue();
    }
}
