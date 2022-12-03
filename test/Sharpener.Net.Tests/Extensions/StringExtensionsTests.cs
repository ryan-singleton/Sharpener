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
        "value".Is("Value").Should().BeFalse();
        "value".NoCase().Is("Value").Should().BeTrue();
        "value".Case().Is("Value").Should().BeFalse();
        "value".NoCase().Ordinal().Is("Value").Should().BeTrue();
        "value".Case().Ordinal().Is("Value").Should().BeFalse();
        "value".NoCase().Current().Is("Value").Should().BeTrue();
        "value".Case().Current().Is("Value").Should().BeFalse();
        "value".NoCase().Invariant().Is("Value").Should().BeTrue();
        "value".Case().Invariant().Is("Value").Should().BeFalse();

        "some-value".Has("Value").Should().BeFalse();
        "some-value".NoCase().Has("Value").Should().BeTrue();
        "some-value".Case().Has("Value").Should().BeFalse();
        "some-value".NoCase().Ordinal().Has("Value").Should().BeTrue();
        "some-value".Case().Ordinal().Has("Value").Should().BeFalse();
        "some-value".NoCase().Current().Has("Value").Should().BeTrue();
        "some-value".Case().Current().Has("Value").Should().BeFalse();
        "some-value".NoCase().Invariant().Has("Value").Should().BeTrue();
        "some-value".Case().Invariant().Has("Value").Should().BeFalse();
    }
}