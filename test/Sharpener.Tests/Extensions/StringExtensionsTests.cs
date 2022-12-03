using FluentAssertions;
using Sharpener.Extensions;
using Xunit;

namespace Sharpener.Tests.Extensions;

public class StringExtensionsTests
{

    [Fact]
    public void Equals_Success()
    {
        _ = "value".Equals("Value").Should().BeFalse();
        _ = "value".NoCase().Equals("Value").Should().BeTrue();
        _ = "value".Case().Equals("Value").Should().BeFalse();
        _ = "value".NoCase().Ordinal().Equals("Value").Should().BeTrue();
        _ = "value".Case().Ordinal().Equals("Value").Should().BeFalse();
        _ = "value".NoCase().Current().Equals("Value").Should().BeTrue();
        _ = "value".Case().Current().Equals("Value").Should().BeFalse();
        _ = "value".NoCase().Invariant().Equals("Value").Should().BeTrue();
        _ = "value".Case().Invariant().Equals("Value").Should().BeFalse();

        _ = "some-value".Contains("Value").Should().BeFalse();
        _ = "some-value".NoCase().Contains("Value").Should().BeTrue();
        _ = "some-value".Case().Contains("Value").Should().BeFalse();
        _ = "some-value".NoCase().Ordinal().Contains("Value").Should().BeTrue();
        _ = "some-value".Case().Ordinal().Contains("Value").Should().BeFalse();
        _ = "some-value".NoCase().Current().Contains("Value").Should().BeTrue();
        _ = "some-value".Case().Current().Contains("Value").Should().BeFalse();
        _ = "some-value".NoCase().Invariant().Contains("Value").Should().BeTrue();
        _ = "some-value".Case().Invariant().Contains("Value").Should().BeFalse();
    }
}
