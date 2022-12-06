// The Sharpener project and Facefire license this file to you under the MIT license.

using Sharpener.Tests.Common.Models;

namespace Sharpener.Tests.Extensions;

public class CollectionExtensionsTests
{
    private readonly IEnumerable<Item> _items;
    public CollectionExtensionsTests() => _items = new[]
        {
            new Item("Item", "FirstItem"),
            new Item("Item", "SecondItem")
        };

    [Fact]
    public void ForAll_Success()
    {
        const string newName = "AffectedItem";
        _items.ForAll(x => x.Name = newName);

        _items.Should().AllSatisfy(x => x.Equals(newName));
    }

    [Fact]
    public void LeftJoin_Success()
    {
        var leftItems = new List<Item>{
            new Item("Bob", "Friend"),
            new Item("Jane", "Friend"),
            new Item("Seth", "Friend"),
            new Item("Gary", "Family"),
            new Item("Shane", "Coworker")
        };

        var rightItems = new List<Item>{
            new Item("Bob", "Friend"),
            new Item("Henry", "Friend"),
            new Item("Will", "Friend"),
            new Item("Gary", "Family"),
            new Item("Shane", "Coworker"),
            new Item("Sally", "Coworker")
        };

        var results = leftItems.LeftJoin(rightItems,
        left => left.Name,
        right => right.Name,
        (left, right) => new { Left = left, Right = right })?.AsList();

        // results.Should().HaveCount()
        results!.Any(x => x.Left.Name.Equals("Bob")).Should().BeTrue();
        results!.Any(x => x.Left.Name.Equals("Jane")).Should().BeTrue();
        results!.Any(x => x.Right is null).Should().BeTrue();
    }
}
