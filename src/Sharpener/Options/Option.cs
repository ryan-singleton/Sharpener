// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Options;

/// <inheritdoc />
public readonly struct Option<T, TAlt> : IOption<T, TAlt>
{
    private readonly T? _value;

    /// <inheritdoc />
    public T? Value => IsSuccess ? _value : default;

    /// <inheritdoc />
    public TAlt? Alternate { get; }

    /// <summary>
    ///     Initializes a successful new instance of the <see cref="Option{T,TAlt}" /> struct with the ability to have a
    ///     primary result value and an alternate value.
    /// </summary>
    /// <param name="value">The primary response value.</param>
    /// <param name="alternate">The alternate value.</param>
    public Option(T? value, TAlt? alternate = default)
    {
        IsSuccess = true;
        _value = value;
        Alternate = alternate;
    }

    /// <summary>
    ///     Initializes an unsuccessful new instance of the <see cref="Option{T,TAlt}" /> struct with only an alternate value.
    /// </summary>
    /// <param name="alternate">The alternate value.</param>
    public Option(TAlt? alternate)
    {
        IsSuccess = false;
        _value = default;
        Alternate = alternate;
    }

    /// <inheritdoc />
    public bool IsSuccess { get; }

    /// <summary>
    ///     Initializes a new <see cref="Option{T,TAlt}" /> when a value is provided and the requirement of a
    ///     <see cref="Option{T,TAlt}" /> is implied.
    /// </summary>
    /// <param name="value">The value to generate a <see cref="Option{T,TAlt}" /> from.</param>
    /// <returns>A successful <see cref="Option{T,TAlt}" /> containing the value.</returns>
    public static implicit operator Option<T, TAlt>(T? value)
    {
        return new Option<T, TAlt>(value);
    }

    /// <summary>
    ///     Initializes a new <see cref="Option{T,TAlt}" /> when values are provided and the requirement of a
    ///     <see cref="Option{T,TAlt}" /> is implied.
    /// </summary>
    /// <param name="tuple">The values to generate a <see cref="Option{T,TAlt}" /> from.</param>
    /// <returns>A successful <see cref="Option{T,TAlt}" /> containing the values.</returns>
    public static implicit operator Option<T, TAlt>((T? value, TAlt? alternate) tuple)
    {
        return new Option<T, TAlt>(tuple.value, tuple.alternate);
    }

    /// <summary>
    ///     Initializes a new <see cref="Option{T,TAlt}" /> when a alternate is provided and the
    ///     requirement of a <see cref="Option{T,TAlt}" /> is implied.
    /// </summary>
    /// <param name="alternate">The alternate to generate a <see cref="Option{T,TAlt}" /> from.</param>
    /// <returns>An unsuccessful <see cref="Option{T,TAlt}" /> containing the alternate.</returns>
    public static implicit operator Option<T, TAlt>(TAlt? alternate)
    {
        return new Option<T, TAlt>(alternate);
    }

    /// <inheritdoc />
    public TResult Resolve<TResult>(Func<T?, TResult> success, Func<TAlt?, TResult> alternate)
    {
        return IsSuccess ? success(_value) : alternate(Alternate);
    }

    /// <inheritdoc />
    public void Resolve(Action<T?> success, Action<TAlt?> alternate)
    {
        if (IsSuccess)
        {
            success(_value);
            return;
        }

        alternate(Alternate);
    }
}
