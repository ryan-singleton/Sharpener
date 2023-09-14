// The Sharpener project licenses this file to you under the MIT license.

using System.Net;
using Sharpener.Rest.Extensions;

namespace Sharpener.Rest.Retry;

/// <summary>
///     Options for retrying an HTTP request.
/// </summary>
public sealed class RetryOptions
{
    /// <summary>
    ///     Instance constructor.
    /// </summary>
    public RetryOptions()
    {
        MaximumAttempts = 3;
        Delay = TimeSpan.FromSeconds(1);
        UseBackoff = true;
        BackoffFactor = 2;
        SetRequirement(message => !message.IsRetryStatusCode());
    }

    /// <summary>
    ///     The maximum number of attempts to retry the request.
    /// </summary>
    public int MaximumAttempts { get; set; }

    /// <summary>
    ///     The delay between attempts.
    /// </summary>
    public TimeSpan Delay { get; set; }

    /// <summary>
    ///     Whether to use backoff when retrying.
    /// </summary>
    public bool UseBackoff { get; set; }

    /// <summary>
    ///     A requirement that must be met for a retry to be avoided.
    /// </summary>
    /// <remarks>
    ///     Defaults to a check for a non-retry status code.
    /// </remarks>
    internal Func<HttpResponseMessage, Task<bool>>? Requirement { get; private set; }

    /// <summary>
    ///     An action to invoke when a retry occurs.
    /// </summary>
    internal Action<int, HttpStatusCode>? Acknowledgement { get; private set; }

    /// <summary>
    ///     The factor to multiply the delay by when using backoff.
    /// </summary>
    public double BackoffFactor { get; set; }

    /// <summary>
    ///     Sets the action to invoke when a retry occurs.
    /// </summary>
    /// <param name="acknowledgement"></param>
    public void SetAcknowledgement(Action<int, HttpStatusCode> acknowledgement)
    {
        Acknowledgement = acknowledgement;
    }

    /// <summary>
    ///     A requirement that must be met for a retry to be avoided.
    /// </summary>
    /// <param name="requirement">
    ///     A requirement that must be met for a retry to be avoided.
    /// </param>
    public void SetRequirement(Func<HttpResponseMessage, Task<bool>> requirement)
    {
        Requirement = requirement;
    }

    /// <summary>
    ///     A requirement that must be met for a retry to be avoided.
    /// </summary>
    /// <param name="requirement">
    ///     A requirement that must be met for a retry to be avoided.
    /// </param>
    public void SetRequirement(Func<HttpResponseMessage, bool> requirement)
    {
        Requirement = message => Task.FromResult(requirement(message));
    }

    /// <summary>
    ///     Updates the delay based on the backoff factor.
    /// </summary>
    internal void UpdateBackoff()
    {
        if (UseBackoff)
        {
            Delay = TimeSpan.FromTicks((long)(Delay.Ticks * BackoffFactor));
        }
    }
}
