// The Sharpener project licenses this file to you under the MIT license.

using System.Net;
using Sharpener.Options;
using Sharpener.Rest.Pagination;

namespace Sharpener.Rest.Tests.Pagination;

public class PaginatedCursorTests
{
    [Fact]
    public void MoveNext_Should_Return_False_When_No_More_Items()
    {
        var funcCalled = false;

        Task<Option<Paginated<string>, HttpResponseMessage>> TestFunc(int _, int __)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            funcCalled = true;
            return Task.FromResult<Option<Paginated<string>, HttpResponseMessage>>(response);
        }

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = cursor.MoveNext;

        result.ShouldBeFalse();
        funcCalled.ShouldBeTrue();
    }

    [Fact]
    public void MoveNext_Should_Return_True_When_More_Items_Are_Available()
    {
        var funcCalled = false;

        Task<Option<Paginated<string>, HttpResponseMessage>> TestFunc(int currentPage, int _)
        {
            var paginated = new Paginated<string>
            {
                Items = new[] { "Item1", "Item2", "Item3" }, CurrentPage = currentPage, HasMore = true
            };
            funcCalled = true;
            return Task.FromResult<Option<Paginated<string>, HttpResponseMessage>>(paginated);
        }

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = cursor.MoveNext;

        result.ShouldBeTrue();
        funcCalled.ShouldBeTrue();
        cursor.Current.Value.ShouldNotBeNull();
        cursor.Current.Value?.Items.ShouldBe(new[] { "Item1", "Item2", "Item3" }, ignoreOrder: true);
        cursor.Current.Value!.CurrentPage.ShouldBe(1);
        cursor.Current.Value!.HasMore.ShouldBeTrue();
    }

    [Fact]
    public async Task MoveNextAsync_Should_Return_False_When_No_More_Items()
    {
        var funcCalled = false;

        Task<Option<Paginated<string>, HttpResponseMessage>> TestFunc(int _, int __)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            funcCalled = true;
            return Task.FromResult<Option<Paginated<string>, HttpResponseMessage>>(response);
        }

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = await cursor.MoveNextAsync();

        result.ShouldBeFalse();
        funcCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task MoveNextAsync_Should_Return_True_When_More_Items_Are_Available()
    {
        var funcCalled = false;

        Task<Option<Paginated<string>, HttpResponseMessage>> TestFunc(int currentPage, int _)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var paginated = new Paginated<string>
            {
                Items = new[] { "Item1", "Item2", "Item3" }, CurrentPage = currentPage, HasMore = true
            };
            funcCalled = true;
            return Task.FromResult(new Option<Paginated<string>, HttpResponseMessage>(paginated, response));
        }

        var cursor = new PaginatedCursor<string>(1, 10, TestFunc);
        var result = await cursor.MoveNextAsync();

        result.ShouldBeTrue();
        funcCalled.ShouldBeTrue();
        cursor.Current.Value.ShouldNotBeNull();
        cursor.Current.Value?.Items.ShouldBe(new[] { "Item1", "Item2", "Item3" }, ignoreOrder: true);
        cursor.Current.Value!.CurrentPage.ShouldBe(1);
        cursor.Current.Value!.HasMore.ShouldBeTrue();
    }
}
