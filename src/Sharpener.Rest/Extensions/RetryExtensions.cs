// The Sharpener project licenses this file to you under the MIT license.

using System.Net;
using Sharpener.Extensions;
using Sharpener.Rest.Retry;

namespace Sharpener.Rest.Extensions;

/// <summary>
///     Extensions for http retry logic.
/// </summary>
public static class RetryExtensions
{
    /// <summary>
    ///     The default retry status codes.
    /// </summary>
    public static readonly HttpStatusCode[] DefaultRetryStatusCodes =
    {
        HttpStatusCode.RequestTimeout, (HttpStatusCode)425, (HttpStatusCode)429, HttpStatusCode.InternalServerError,
        HttpStatusCode.BadGateway, HttpStatusCode.ServiceUnavailable, HttpStatusCode.GatewayTimeout
    };

    /// <summary>
    ///     Determines if the response status code indicates that the request should be retried.
    /// </summary>
    /// <remarks>
    ///     If not provided, the default retry codes are: 408, 425, 429, 500, 502, 503, 504.
    /// </remarks>
    /// <param name="httpResponseMessage">
    ///     The <see cref="HttpResponseMessage" /> to check.
    /// </param>
    /// <param name="retryCodes">
    ///     The HTTP status codes that will trigger a retry.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the response status code indicates that the request should be retried; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsRetryStatusCode(this HttpResponseMessage httpResponseMessage,
        params HttpStatusCode[]? retryCodes)
    {
        retryCodes = retryCodes.IsNullOrEmpty() ? DefaultRetryStatusCodes : retryCodes;
        return retryCodes.Contains(httpResponseMessage.StatusCode);
    }

    /// <summary>
    ///     Retries an HTTP request based on the provided options.
    /// </summary>
    /// <param name="func">The function to retry.</param>
    /// <param name="configure">The configuration of the retry options.</param>
    /// <returns>A response when the task eventually succeeds or exceeds the max retry.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<HttpResponseMessage> WithRetry(this Func<Task<HttpResponseMessage>> func,
        Action<RetryOptions>? configure = null)
    {
        var options = new RetryOptions();
        configure?.Invoke(options);

        return await func.WithRetry(options).ConfigureAwait(false);
    }

    /// <summary>
    ///     Retries an HTTP request based on the provided options.
    /// </summary>
    /// <param name="func">The function to retry.</param>
    /// <param name="options">The options for the retries.</param>
    /// <returns>A response when the task eventually succeeds or exceeds the max retry.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<HttpResponseMessage> WithRetry(this Func<Task<HttpResponseMessage>> func,
        RetryOptions options)
    {
        var attempts = 0;
        var response = await func().ConfigureAwait(false);

        while (attempts < options.MaximumAttempts && options.Requirement is not null &&
               !await options.Requirement(response).ConfigureAwait(false))
        {
            attempts++;
            options.Acknowledgement?.Invoke(attempts, response.StatusCode);
            await Task.Delay(options.Delay).ConfigureAwait(false);

            options.UpdateBackoff();
            response = await func().ConfigureAwait(false);
        }

        return response;
    }
}
