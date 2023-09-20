// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Types.Strings;
using Sharpener.Types.Strings.Interfaces;

namespace Sharpener.Extensions;

/// <summary>
///     Extensions for strings.
/// </summary>
public static class StringExtensions
{
    /// <inheritdoc cref="IStringComparer.Case" />
    public static IStringComparer Case(this string source)
    {
        return new CaseStringComparer(source, false);
    }

    /// <inheritdoc cref="IStringComparer.Current" />
    public static IStringComparer Current(this string source)
    {
        return new CultureStringComparer(source, StringComparison.CurrentCulture);
    }

    /// <inheritdoc cref="IStringComparer.Invariant" />
    public static IStringComparer Invariant(this string source)
    {
        return new CultureStringComparer(source, StringComparison.InvariantCulture);
    }

    /// <inheritdoc cref="IStringComparer.NoCase" />
    public static IStringComparer NoCase(this string source)
    {
        return new CaseStringComparer(source, true);
    }

    /// <inheritdoc cref="IStringComparer.Ordinal" />
    public static IStringComparer Ordinal(this string source)
    {
        return new CultureStringComparer(source, StringComparison.Ordinal);
    }

    /// <summary>
    ///     <inheritdoc cref="Environment.GetEnvironmentVariable(string, EnvironmentVariableTarget)" />
    /// </summary>
    /// <param name="environmentVariable">The name of the environment variable.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget" /> values.</param>
    /// <returns>
    ///     The value of the environment variable specified by the variable and target parameters, or null if the environment
    ///     variable is not found.
    /// </returns>
    public static string? GetEnvironmentVariable(this string environmentVariable,
        EnvironmentVariableTarget? target = null) =>
        target is not null
            ? Environment.GetEnvironmentVariable(environmentVariable, (EnvironmentVariableTarget)target)
            : Environment.GetEnvironmentVariable(environmentVariable);

    /// <summary>
    ///     Returns the provided string unchanged unless it is null, empty, or whitespace. In which case, the fallback is
    ///     provided.
    /// </summary>
    /// <remarks>
    ///     Useful for situations where a replacement value is needed when encountering null, empty, or whitespace strings. For
    ///     example, generating a hash based on properties of a data model. You may want to represent fields that are null or
    ///     empty with a fallback value. <br />
    ///     By default, if the fallback is not provided, <see cref="SharpenerStringsSettings.DefaultFallback" />  is returned.
    ///     You can override the fallback to be an empty string or a specific whitespace to normalize empty/whitespace values.
    ///     But by default, they will be converted to <see cref="SharpenerStringsSettings.DefaultFallback" />.
    /// </remarks>
    /// <param name="value">The value to be evaluated for null, empty, or whitespace</param>
    /// <param name="fallback">The value to provide when null, empty, or whitespace</param>
    /// <returns>A string that is not null and only empty or whitespace if specified</returns>
    public static string OrFallback(this string? value, string? fallback = null)
    {
        fallback ??= SharpenerStringsSettings.DefaultFallback ?? throw new NullReferenceException("The fallback value was null and no default was set.");
        return (string.IsNullOrWhiteSpace(value) ? fallback : value)!;
    }
}
