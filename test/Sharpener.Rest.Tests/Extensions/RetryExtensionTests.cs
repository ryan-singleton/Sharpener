// The Sharpener project licenses this file to you under the MIT license.

using FluentAssertions;
using Sharpener.Rest.Retry;

namespace Sharpener.Rest.Tests.Extensions;

public class RetryExtensionTests
{
    [Fact]
    public void RetryOptions_DefaultValues()
    {
        var options = new RetryOptions();

        options.MaximumAttempts.Should().Be(3);
        options.Delay.Should().Be(TimeSpan.FromSeconds(1));
        options.UseBackoff.Should().BeTrue();
        options.Acknowledgement.Should().BeNull();
        options.BackoffFactor.Should().Be(2);
    }

    [Fact]
    public void RetryOptions_UpdateBackoff()
    {
        var options = new RetryOptions
        {
            Delay = TimeSpan.FromSeconds(1),
            UseBackoff = true,
            BackoffFactor = 2
        };

        options.UpdateBackoff();

        options.Delay.Should().Be(TimeSpan.FromSeconds(2));

        options.UpdateBackoff();

        options.Delay.Should().Be(TimeSpan.FromSeconds(4));
    }
}
