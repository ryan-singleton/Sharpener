// The Sharpener project licenses this file to you under the MIT license.

using System.Globalization;
using System.Text;
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
        EnvironmentVariableTarget? target = null)
    {
        return target is not null
            ? Environment.GetEnvironmentVariable(environmentVariable, (EnvironmentVariableTarget)target)
            : Environment.GetEnvironmentVariable(environmentVariable);
    }

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
    public static string? OrFallback(this string? value, string? fallback = null)
    {
        fallback ??= SharpenerStringsSettings.DefaultFallback ??
                     throw new NullReferenceException("The fallback value was null and no default was set.");
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }

    /// <summary>
    ///     Returns whether the string contains any of the values provided.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool ContainsAny(this string value, params string[] values) => values.Any(value.Contains);

    /// <summary>
    ///     Returns whether the string contains any of the values provided. Case insensitive.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool NoCaseContainsAny(this string value, params string[] values) =>
        values.Any(val => value.Contains(val, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    ///     Returns whether the string contains all the values provided.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool ContainsAll(this string value, params string[] values) => values.All(value.Contains);

    /// <summary>
    ///     Returns whether the string contains all the values provided. Case insensitive.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool NoCaseContainsAll(this string value, params string[] values) =>
        values.All(val => value.Contains(val, StringComparison.OrdinalIgnoreCase));

    /// <inheritdoc cref="StringBuilder.AppendLine()" />
    /// <remarks>
    ///     This and similar methods are part of a suite of helper methods that automatically account for .NET SDK style
    ///     libraries that must accomodate early methods and newer methods that must provide a format provider, defaulting to
    ///     <see cref="CultureInfo.CurrentCulture" /> where possible.
    /// </remarks>
    public static StringBuilder AppendWith(this StringBuilder stringBuilder) => stringBuilder.AppendLine();

    /// <inheritdoc cref="StringBuilder.AppendLine(string)" />
    /// <remarks>
    ///     This and similar methods are part of a suite of helper methods that automatically account for .NET SDK style
    ///     libraries that must accomodate early methods and newer methods that must provide a format provider, defaulting to
    ///     <see cref="CultureInfo.CurrentCulture" /> where possible.
    /// </remarks>
    public static StringBuilder AppendWith(this StringBuilder stringBuilder, string? value) =>
        stringBuilder.AppendLine(value);

    /// <inheritdoc cref="StringBuilder.AppendLine(string)" />
    /// <remarks>
    ///     This and similar methods are part of a suite of helper methods that automatically account for .NET SDK style
    ///     libraries that must accomodate early methods and newer methods that must provide a format provider, defaulting to
    ///     <see cref="CultureInfo.CurrentCulture" /> where possible.
    /// </remarks>
    public static StringBuilder AppendWith(this StringBuilder stringBuilder, FormattableString formattable,
        CultureInfo? culture = null) =>
        stringBuilder.AppendLine(formattable.ToString(culture ?? CultureInfo.CurrentCulture));
}
