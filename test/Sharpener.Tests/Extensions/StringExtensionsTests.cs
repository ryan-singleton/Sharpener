// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public async Task Equals_Parallel_Success()
    {
        var tasks = new List<Task<bool>>
        {
            Task.Run(() => "value".Equals("Value")),
            Task.Run(() => "value".NoCase().Equals("Value")),
            Task.Run(() => "value".Case().Equals("Value")),
            Task.Run(() => "value".NoCase().Ordinal().Equals("Value")),
            Task.Run(() => "value".Case().Ordinal().Equals("Value")),
            Task.Run(() => "value".NoCase().Current().Equals("Value")),
            Task.Run(() => "value".Case().Current().Equals("Value")),
            Task.Run(() => "value".NoCase().Invariant().Equals("Value")),
            Task.Run(() => "value".Case().Invariant().Equals("Value"))
        };
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        var expectedResults = new List<bool>
        {
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false
        };
        results.Length.ShouldBe(expectedResults.Count);
        for (var i = 0; i < results.Length; i++)
        {
            var result = results[i];
            var expectedResult = expectedResults[i];
            result.ShouldBe(expectedResult);
        }
    }

    [Fact]
    public void Equals_Success()
    {
        "value".Equals("Value").ShouldBeFalse();
        "value".NoCase().Equals("Value").ShouldBeTrue();
        "value".Case().Equals("Value").ShouldBeFalse();
        "value".NoCase().Ordinal().Equals("Value").ShouldBeTrue();
        "value".Case().Ordinal().Equals("Value").ShouldBeFalse();
        "value".NoCase().Current().Equals("Value").ShouldBeTrue();
        "value".Case().Current().Equals("Value").ShouldBeFalse();
        "value".NoCase().Invariant().Equals("Value").ShouldBeTrue();
        "value".Case().Invariant().Equals("Value").ShouldBeFalse();
        "some-value".Contains("Value").ShouldBeFalse();
        "some-value".NoCase().Contains("Value").ShouldBeTrue();
        "some-value".Case().Contains("Value").ShouldBeFalse();
        "some-value".NoCase().Ordinal().Contains("Value").ShouldBeTrue();
        "some-value".Case().Ordinal().Contains("Value").ShouldBeFalse();
        "some-value".NoCase().Current().Contains("Value").ShouldBeTrue();
        "some-value".Case().Current().Contains("Value").ShouldBeFalse();
        "some-value".NoCase().Invariant().Contains("Value").ShouldBeTrue();
        "some-value".Case().Invariant().Contains("Value").ShouldBeFalse();
    }
}
