using Sharpener.Tests.Models;
using Sharpener.Extensions;
using FluentAssertions;

namespace Sharpener.Tests.Extensions;

public class CollectionExtensionsTests
{
    IEnumerable<Item> _items;
    public CollectionExtensionsTests()
    {
        _items = new[]
        {
            new Item("Item", "FirstItem"),
            new Item("Item", "SecondItem")
        };
    }

    [Fact]
    public void ForAll_Success()
    {
        const string newName = "AffectedItem";
        _items.ForAll(x => x.Name = newName);

        _items.Should().AllSatisfy(x => x.Equals(newName));
    }
}
