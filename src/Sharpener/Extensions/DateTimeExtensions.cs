// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Extensions;

/// <summary>
///     Extensions for <see cref="DateTime" /> and <see cref="TimeSpan" /> related operations.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    ///     Converts the ticks to total days.
    /// </summary>
    /// <param name="ticks">The ticks to convert.</param>
    /// <returns></returns>
    public static double TicksToDays(this long ticks)
    {
        return TimeSpan.FromTicks(ticks).TotalDays;
    }

    /// <summary>
    ///     Converts the ticks to total hours.
    /// </summary>
    /// <param name="ticks">The ticks to convert.</param>
    /// <returns></returns>
    public static double TicksToHours(this long ticks)
    {
        return TimeSpan.FromTicks(ticks).TotalHours;
    }

    /// <summary>
    ///     Converts the ticks to total milliseconds.
    /// </summary>
    /// <param name="ticks">The ticks to convert.</param>
    /// <returns></returns>
    public static double TicksToMilliseconds(this long ticks)
    {
        return TimeSpan.FromTicks(ticks).TotalMilliseconds;
    }

    /// <summary>
    ///     Converts the ticks to total minutes.
    /// </summary>
    /// <param name="ticks">The ticks to convert.</param>
    /// <returns></returns>
    public static double TicksToMinutes(this long ticks)
    {
        return TimeSpan.FromTicks(ticks).TotalMinutes;
    }

    /// <summary>
    ///     Converts the ticks to total seconds.
    /// </summary>
    /// <param name="ticks">The ticks to convert.</param>
    /// <returns></returns>
    public static double TicksToSeconds(this long ticks)
    {
        return TimeSpan.FromTicks(ticks).TotalSeconds;
    }
}
