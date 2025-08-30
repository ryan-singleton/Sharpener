// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Results;

namespace Sharpener.Extensions;

/// <summary>
///     Extensions for <see cref="Outcome{TSuccess}" />.
/// </summary>
public static class OutcomeExtensions
{
    /// <summary>
    ///     Implicit conversion of a <see cref="Outcome{T}" /> can sometimes be difficult when dealing with generics,
    ///     interfaces, or combinations of both. Use this after handling your error logic to force a conversion to whatever
    ///     your return type is. For example, the IEnumerable generic type can be challenging to return, so call this once
    ///     you have your success result.
    /// </summary>
    /// <param name="value">The success result.</param>
    /// <typeparam name="TSuccess">
    ///     The type of the success result, often inferred from <paramref name="value" /> and can therefore be omitted.
    /// </typeparam>
    /// <returns>The success value contained in a result.</returns>
    public static Outcome<TSuccess> ToOutcome<TSuccess>(this TSuccess value)
    {
        return value;
    }

    /// <summary>
    ///     Creates a failed <see cref="Outcome{T}"/> with the specified error message.
    ///     This is useful when you want to produce an <see cref="Outcome{T}"/> representing an error,
    ///     even in contexts where you might have a value of type <typeparamref name="T"/> available.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the success value that the <see cref="Outcome{T}"/> would hold on success.
    ///     In this case, the <see cref="Outcome{T}"/> will represent a failure instead.
    /// </typeparam>
    /// <param name="value">
    ///     The success value is ignored; it is only used to satisfy extension method syntax.
    ///     The returned <see cref="Outcome{T}"/> will always be a failure.
    /// </param>
    /// <param name="errorMessage">
    ///     The error message that will be wrapped in an <see cref="Error"/> and returned as a failed outcome.
    /// </param>
    /// <returns>
    ///     An <see cref="Outcome{T}"/> representing a failure with the provided error message.
    /// </returns>
    public static Outcome<T> ToOutcomeError<T>(this T value, string errorMessage)
    {
        return new Error(errorMessage); // implicit conversion to Outcome<T>
    }
}
