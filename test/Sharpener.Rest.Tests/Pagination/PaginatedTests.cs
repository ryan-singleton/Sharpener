// The Sharpener project licenses this file to you under the MIT license.

using FluentAssertions;
using Sharpener.Rest.Pagination;

namespace Sharpener.Rest.Tests.Pagination;

public class PaginatedTests
{
    [Fact]
    public void Paginated_HasMore_ReturnsFalse()
    {
        var paginated = new Paginated<string> { HasMore = false };
        var result = paginated.HasMore;
        result.Should().BeFalse();
    }

    [Fact]
    public void Paginated_HasMore_ReturnsTrue()
    {
        var paginated = new Paginated<string> { HasMore = true };
        var result = paginated.HasMore;
        result.Should().BeTrue();
    }

    [Fact]
    public void Paginated_InitializesWithEmptyItems()
    {
        var paginated = new Paginated<string>();
        var items = paginated.Items;
        items.Should().BeEmpty();
    }

    [Fact]
    public void Paginated_NextPage_ReturnsNextPageNumber()
    {
        const int currentPage = 2;
        var paginated = new Paginated<string>
        {
            CurrentPage = currentPage,
            HasMore = true
        };
        var nextPage = paginated.NextPage;
        nextPage.Should().Be(currentPage + 1);
    }

    [Fact]
    public void Paginated_NextPage_ReturnsNullForLastPage()
    {
        const int currentPage = 5;
        var paginated = new Paginated<string>
        {
            CurrentPage = currentPage,
            HasMore = false
        };
        var nextPage = paginated.NextPage;
        nextPage.Should().BeNull();
    }

    [Fact]
    public void Paginated_SetsCurrentPage()
    {
        const int currentPage = 3;
        var paginated = new Paginated<string> { CurrentPage = currentPage };
        var result = paginated.CurrentPage;
        result.Should().Be(currentPage);
    }
}
