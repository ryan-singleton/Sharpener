using Sharpener.Net.Tests.Models;
using Sharpener.Net.Extensions;
using FluentAssertions;
using Sharpener.Net.Preferences;

namespace Sharpener.Net.Tests.Extensions;

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
}
