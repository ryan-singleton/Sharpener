// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Options;

/// <summary>
///     Represents a result that can contain an expected "success" result, or an alternate fallback result. The alternate
///     can be any type and need not be an exception.
/// </summary>
/// <typeparam name="T">The expected "success" result type.</typeparam>
/// <typeparam name="TAlt">The alternate result type when unsuccessful or optional when successful.</typeparam>
public interface IOption<out T, out TAlt>
{
    /// <summary>
    ///     The successful value if the result is a success; otherwise, returns null.
    /// </summary>
    T? Value { get; }

    /// <summary>
    ///     The alternate value. May be present regardless of success.
    /// </summary>
    TAlt? Alternate { get; }

    /// <summary>
    ///     Gets a value indicating whether the primary result is a success.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    ///     Resolves the result and executes the appropriate function based on whether it is a success or a failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the executed function.</typeparam>
    /// <param name="success">The function to execute if the result is a success.</param>
    /// <param name="alternate">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    TResult Resolve<TResult>(Func<T?, TResult> success, Func<TAlt?, TResult> alternate);

    /// <summary>
    ///     Resolves the result and executes the appropriate action based on whether it is a success or a failure.
    /// </summary>
    /// <param name="success">The function to execute if the result is a success.</param>
    /// <param name="alternate">The function to execute if the result is a failure.</param>
    void Resolve(Action<T?> success, Action<TAlt?> alternate);
}
