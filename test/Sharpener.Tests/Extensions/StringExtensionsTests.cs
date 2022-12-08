// The Sharpener project and Facefire license this file to you under the MIT license.
namespace Sharpener.Tests.Extensions;
public class StringExtensionsTests
{
    [Fact]
    public void Equals_Success()
    {
        "value".Equals("Value").Should().BeFalse();
        "value".NoCase().Equals("Value").Should().BeTrue();
        "value".Case().Equals("Value").Should().BeFalse();
        "value".NoCase().Ordinal().Equals("Value").Should().BeTrue();
        "value".Case().Ordinal().Equals("Value").Should().BeFalse();
        "value".NoCase().Current().Equals("Value").Should().BeTrue();
        "value".Case().Current().Equals("Value").Should().BeFalse();
        "value".NoCase().Invariant().Equals("Value").Should().BeTrue();
        "value".Case().Invariant().Equals("Value").Should().BeFalse();
        "some-value".Contains("Value").Should().BeFalse();
        "some-value".NoCase().Contains("Value").Should().BeTrue();
        "some-value".Case().Contains("Value").Should().BeFalse();
        "some-value".NoCase().Ordinal().Contains("Value").Should().BeTrue();
        "some-value".Case().Ordinal().Contains("Value").Should().BeFalse();
        "some-value".NoCase().Current().Contains("Value").Should().BeTrue();
        "some-value".Case().Current().Contains("Value").Should().BeFalse();
        "some-value".NoCase().Invariant().Contains("Value").Should().BeTrue();
        "some-value".Case().Invariant().Contains("Value").Should().BeFalse();
    }
    [Fact]
    public async void Equals_Parallel_Success()
    {
        var tasks = new List<Task<bool>>();
        tasks.Add(Task.Run(() => "value".Equals("Value")));
        tasks.Add(Task.Run(() => "value".NoCase().Equals("Value")));
        tasks.Add(Task.Run(() => "value".Case().Equals("Value")));
        tasks.Add(Task.Run(() => "value".NoCase().Ordinal().Equals("Value")));
        tasks.Add(Task.Run(() => "value".Case().Ordinal().Equals("Value")));
        tasks.Add(Task.Run(() => "value".NoCase().Current().Equals("Value")));
        tasks.Add(Task.Run(() => "value".Case().Current().Equals("Value")));
        tasks.Add(Task.Run(() => "value".NoCase().Invariant().Equals("Value")));
        tasks.Add(Task.Run(() => "value".Case().Invariant().Equals("Value")));
        var results = await Task.WhenAll<bool>(tasks);
        var expectedResults = new List<bool>()
        {
            false, true, false, true, false, true, false, true, false
        };
        results.Should().HaveSameCount(expectedResults);
        for (var i = 0; i < results.Length; i++)
        {
            var result = results[i];
            var expectedResult = expectedResults[i];
            result.Should().Be(expectedResult);
        }
    }
}
