using FluentAssertions;
using Sharpener.Extensions;
using Sharpener.Tests.Models;
using Xunit;

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

        _ = _items.Should().AllSatisfy(x => x.Equals(newName));
    }
}
