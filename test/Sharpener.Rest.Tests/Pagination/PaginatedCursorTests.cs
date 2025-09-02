// The Sharpener project licenses this file to you under the MIT license.

using System.Net;
using Sharpener.Extensions;
using Sharpener.Rest.Pagination;
using Sharpener.Results;

namespace Sharpener.Rest.Tests.Pagination;

public class PaginatedCursorTests
{
    [Fact]
    public void MoveNext_Should_Return_False_When_No_More_Items()
    {
        var funcCalled = false;

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = cursor.MoveNext;

        result.ShouldBeFalse();
        funcCalled.ShouldBeTrue();
        return;

        Task<Outcome<Paginated<string>>> TestFunc(int _, int __)
        {
            funcCalled = true;
            return Task.FromResult(new Paginated<string>().ToOutcomeError("Failed test"));
        }
    }

    [Fact]
    public void MoveNext_Should_Return_True_When_More_Items_Are_Available()
    {
        var funcCalled = false;

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = cursor.MoveNext;

        result.ShouldBeTrue();
        funcCalled.ShouldBeTrue();
        cursor.Current.Value.ShouldNotBeNull();
        cursor.Current.Value?.Items.ShouldContain("Item1");
        cursor.Current.Value?.Items.ShouldContain("Item2");
        cursor.Current.Value?.Items.ShouldContain("Item3");
        cursor.Current.Value!.CurrentPage.ShouldBe(1);
        cursor.Current.Value!.HasMore.ShouldBeTrue();
        return;

        Task<Outcome<Paginated<string>>> TestFunc(int currentPage, int _)
        {
            var paginated = new Paginated<string>
            {
                Items = ["Item1", "Item2", "Item3"], CurrentPage = currentPage, HasMore = true
            };
            funcCalled = true;
            return Task.FromResult<Outcome<Paginated<string>>>(paginated);
        }
    }

    [Fact]
    public async Task MoveNextAsync_Should_Return_False_When_No_More_Items()
    {
        var funcCalled = false;

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = await cursor.MoveNextAsync().ConfigureAwait(false);

        result.ShouldBeFalse();
        funcCalled.ShouldBeTrue();
        return;

        Task<Outcome<Paginated<string>>> TestFunc(int _, int __)
        {
            funcCalled = true;
            return Task.FromResult(new Paginated<string>().ToOutcomeError("Failed test"));
        }
    }

    [Fact]
    public async Task MoveNextAsync_Should_Return_True_When_More_Items_Are_Available()
    {
        var funcCalled = false;

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = await cursor.MoveNextAsync().ConfigureAwait(false);

        result.ShouldBeTrue();
        funcCalled.ShouldBeTrue();
        cursor.Current.Value.ShouldNotBeNull();
        cursor.Current.Value?.Items.ShouldContain("Item1");
        cursor.Current.Value?.Items.ShouldContain("Item2");
        cursor.Current.Value?.Items.ShouldContain("Item3");
        cursor.Current.Value!.CurrentPage.ShouldBe(1);
        cursor.Current.Value!.HasMore.ShouldBeTrue();
        return;

        Task<Outcome<Paginated<string>>> TestFunc(int currentPage, int _)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var paginated = new Paginated<string>
            {
                Items = ["Item1", "Item2", "Item3"], CurrentPage = currentPage, HasMore = true
            };
            funcCalled = true;
            return Task.FromResult(paginated.ToOutcome());
        }
    }
}
