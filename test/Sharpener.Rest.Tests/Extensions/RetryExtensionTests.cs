// The Sharpener project licenses this file to you under the MIT license.


using Sharpener.Rest.Retry;

namespace Sharpener.Rest.Tests.Extensions;

public class RetryExtensionTests
{
    [Fact]
    public void RetryOptions_DefaultValues()
    {
        var options = new RetryOptions();

        options.MaximumAttempts.ShouldBe(3);
        options.Delay.ShouldBe(TimeSpan.FromSeconds(1));
        options.UseBackoff.ShouldBeTrue();
        options.Acknowledgement.ShouldBeNull();
        options.BackoffFactor.ShouldBe(2);
    }

    [Fact]
    public void RetryOptions_UpdateBackoff()
    {
        var options = new RetryOptions { Delay = TimeSpan.FromSeconds(1), UseBackoff = true, BackoffFactor = 2 };

        options.UpdateBackoff();

        options.Delay.ShouldBe(TimeSpan.FromSeconds(2));

        options.UpdateBackoff();

        options.Delay.ShouldBe(TimeSpan.FromSeconds(4));
    }
}
