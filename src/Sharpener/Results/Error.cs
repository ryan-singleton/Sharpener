// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Results;

/// <summary>
///     The error type associated with <see cref="Outcome{T}" /> for a success/error return pattern. This should implicitly
///     convert to and from string values fairly easily, but you can also manually perform the conversions.
/// </summary>
public struct Error
{
    /// <summary>
    ///     Represents the message describing the error in detail.
    ///     This field is read-only and initialized through the constructor of the <see cref="Error" /> struct.
    /// </summary>
    public readonly string ErrorMessage;

    /// <summary>
    ///     Represents an error value associated with operations that yield success or error outcomes.
    ///     Provides implicit conversion to and from string values for convenience.
    /// </summary>
    public Error(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    ///     Implicit conversion to error value.
    /// </summary>
    public static implicit operator string(Error error)
    {
        return error.ErrorMessage;
    }

    /// <summary>
    ///     Implicit conversion from error value.
    /// </summary>
    public static implicit operator Error(string errorMessage)
    {
        return new Error(errorMessage);
    }

    /// <summary>
    ///     Returns a string that represents the current error message.
    /// </summary>
    /// <returns>
    ///     A string containing the error message associated with the current instance.
    /// </returns>
    public override string ToString()
    {
        return ErrorMessage;
    }
}
