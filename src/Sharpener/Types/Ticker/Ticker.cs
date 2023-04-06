// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Types.Ticker;

/// <summary>
///     A struct based and disposable utility for tracking elapsed time during operations. Disposable is mostly used to
///     allow greater control of when it should fall out of scope for cleanup, but it also resets the start time before
///     doing so.
/// </summary>
public struct Ticker : IDisposable
{
    private long _startTicks = long.MinValue;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="start">
    ///     The option to start the ticker immediately. Defaults to true. If not, it must be started before
    ///     calling for elapsed ticks.
    /// </param>
    public Ticker(bool start = true)
    {
        if (start)
        {
            _startTicks = UtcNowTicks;
        }
    }

    /// <summary>
    ///     Starts the ticker.
    /// </summary>
    public void Start()
    {
        _startTicks = UtcNowTicks;
    }

    /// <summary>
    ///     Ticks since the ticker was started. Will throw an exception if the ticker has not been started already which occurs
    ///     by default in the constructor but can be turned off.
    /// </summary>
    /// <returns></returns>
    public long ElapsedTicks => _startTicks is not long.MinValue
        ? UtcNowTicks - _startTicks
        : throw new NotSupportedException("Start the ticker before getting elapsed ticks.");

    /// <summary>
    ///     The current UTC now in ticks.
    /// </summary>
    private static long UtcNowTicks => DateTime.UtcNow.Ticks;

    /// <summary>
    ///     Disposal logic of the ticker.
    /// </summary>
    public void Dispose()
    {
        _startTicks = long.MinValue;
    }
}
