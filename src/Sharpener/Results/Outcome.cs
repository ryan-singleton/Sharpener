// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Results;

/// <summary>
///     A result type that encapsulates success/failure states with implicit conversions
/// </summary>
/// <typeparam name="TSuccess">Type returned on success</typeparam>
public readonly struct Outcome<TSuccess>
{
    private readonly Error _error;
    private readonly TSuccess _value;

    /// <summary>
    ///     Gets the error value (throws if success)
    /// </summary>
    public Error Error => !IsSuccess ? _error : throw new InvalidOperationException("Outcome is in success state");

    /// <summary>
    ///     Returns true if the result represents an error
    /// </summary>
    public bool IsError => !IsSuccess;

    /// <summary>
    ///     Returns true if the result represents success
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    ///     Gets the success value (throws if error)
    /// </summary>
    public TSuccess Value => IsSuccess ? _value : throw new InvalidOperationException("Outcome is in error state");

    private Outcome(TSuccess value)
    {
        IsSuccess = true;
        _value = value;
        _error = default!;
    }

    private Outcome(Error error)
    {
        IsSuccess = false;
        _value = default!;
        _error = error;
    }

    /// <summary>
    ///     Maps the result to a common return type based on success/failure
    /// </summary>
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<Error, TResult> onError)
    {
        return IsSuccess ? onSuccess(_value) : onError(_error);
    }

    /// <summary>
    ///     Executes different actions based on success/failure (for void operations)
    /// </summary>
    public void Match(Action<TSuccess> onSuccess, Action<Error> onError)
    {
        if (IsSuccess)
        {
            onSuccess(_value);
        }
        else
        {
            onError(_error);
        }
    }

    /// <summary>
    ///     Implicit conversion from success value
    /// </summary>
    public static implicit operator Outcome<TSuccess>(TSuccess success)
    {
        return new Outcome<TSuccess>(success);
    }

    /// <summary>
    ///     Implicit conversion from error value
    /// </summary>
    public static implicit operator Outcome<TSuccess>(Error error)
    {
        return new Outcome<TSuccess>(error);
    }

    /// <summary>
    ///     Implicit conversion to success value
    /// </summary>
    public static implicit operator TSuccess(Outcome<TSuccess> outcome)
    {
        return outcome.Value;
    }
}
