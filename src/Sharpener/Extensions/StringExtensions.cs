// The Sharpener project licenses this file to you under the MIT license.

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Sharpener.Types.Strings;
using Sharpener.Types.Strings.Interfaces;

namespace Sharpener.Extensions;

/// <summary>
///     Extensions for strings.
/// </summary>
public static class StringExtensions
{
    private const int MaxSlugLength = 26;

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
    ///     Whether the entire string has uppercase characters.
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns></returns>
    public static bool IsAllUppercase(this string value)
    {
        return value.All(c => !char.IsLetter(c) || char.IsUpper(c));
    }

    /// <summary>
    ///     Whether the string has leading or trailing whitespace.
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns></returns>
    public static bool IsTrimmed(this string value)
    {
        return !value.StartsWith(' ') && !value.EndsWith(' ');
    }

    /// <summary>
    ///     Splits a string at each occurrence of the provided delimiter. Spaces are trimmed and if the delimiter is not
    ///     provided, it defaults to a comma.
    /// </summary>
    /// <param name="delimiter">Optional delimiter. Defaults to a comma.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>A collection of trimmed strings that are neither null nor empty.</returns>
    public static IEnumerable<string> SplitByDelimiter(this string value, char delimiter = ',')
    {
        return value.Split(delimiter).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s));
    }

    /// <summary>
    ///     Converts a string to PascalCase (e.g., "hello world" becomes "HelloWorld")
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var words = Regex.Split(value, @"[^a-zA-Z0-9]+")
            .Where(w => !string.IsNullOrEmpty(w));

        var result = new StringBuilder();
        foreach (var word in words)
        {
            if (word.Length <= 0)
            {
                continue;
            }

            result.Append(char.ToUpper(word[0], CultureInfo.InvariantCulture));
            if (word.Length > 1)
            {
                result.Append(word.Substring(1).ToLower(CultureInfo.InvariantCulture));
            }
        }

        return result.ToString();
    }

    /// <summary>
    ///     Converts a string to camelCase (e.g., "hello world" becomes "helloWorld")
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The resulting camel case string.</returns>
    public static string ToCamelCase(this string value)
    {
        var pascalCase = value.ToPascalCase();
        if (string.IsNullOrEmpty(pascalCase))
        {
            return pascalCase;
        }

        return char.ToLower(pascalCase[0], CultureInfo.InvariantCulture) + pascalCase.Substring(1);
    }

    /// <summary>
    ///     Splits a string at each uppercase letter (except the first) with a space (e.g., "HelloWorld" becomes "Hello World")
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The resulting string with spaces before each non leading upper case.</returns>
    public static string SplitAtUpper(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var result = new StringBuilder();

        for (var i = 0; i < value.Length; i++)
        {
            if (i > 0 && char.IsUpper(value[i]) && !char.IsUpper(value[i - 1]))
            {
                result.Append(' ');
            }

            result.Append(value[i]);
        }

        return result.ToString();
    }

    /// <summary>
    ///     Converts a string to snake_case (e.g., "Hello World" becomes "hello_world")
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The resulting snake case string.</returns>
    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var withSpaces = value.SplitAtUpper();
        return Regex.Replace(withSpaces, @"[^a-zA-Z0-9]+", "_")
            .Trim('_')
            .ToLower(CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Converts a string to kebab-case (e.g., "Hello World" becomes "hello-world")
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The resulting kebab-case string.</returns>
    public static string ToKebabCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var withSpaces = value.SplitAtUpper();
        return Regex.Replace(withSpaces, @"[^a-zA-Z0-9]+", "-")
            .Trim('-')
            .ToLower(CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Converts a string to Title Case (e.g., "hello world" becomes "Hello World")
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The resulting title case string.</returns>
    public static string ToTitleCase(this string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? value
            : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    ///     Checks if string contains only letters and digits
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True if it contains only letters and digits.</returns>
    public static bool IsAlphaNumeric(this string value)
    {
        return !string.IsNullOrEmpty(value) && value.All(char.IsLetterOrDigit);
    }

    /// <summary>
    ///     Checks if string contains only digits
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True if it contains only digits.</returns>
    public static bool IsNumeric(this string value)
    {
        return !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
    }

    /// <summary>
    ///     Gets only the numeric values from the string, but performs no conversions.
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>A string with only numeric values, or an empty string if none are in it.</returns>
    public static string ToNumeric(this string value)
    {
        return value.IsNumeric() ? value : new string(value.Where(c => char.IsDigit(c) || c == '.').ToArray());
    }

    /// <summary>
    ///     Removes all whitespace from the string.
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The string with whitespace removed.</returns>
    public static string RemoveWhitespace(this string value)
    {
        return string.IsNullOrEmpty(value)
            ? value
            : new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    /// <summary>
    ///     Reverses the string.
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The reversed string.</returns>
    public static string Reverse(this string value)
    {
        return string.IsNullOrEmpty(value) ? value : new string(value.Reverse().ToArray());
    }

    /// <summary>
    ///     Truncates string to specified length and adds ellipsis if needed.
    /// </summary>
    /// <param name="input">The string to truncate.</param>
    /// <param name="maxLength">The length at which point to truncate.</param>
    /// <param name="suffix">The string to use as an indication of truncation. Defaults to "..."</param>
    /// <returns>The truncated string if and only if its length exceeds the maxLength.</returns>
    public static string Truncate(this string input, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
        {
            return input;
        }

#if !NET5_0_OR_GREATER
        return input[..(maxLength - suffix.Length)] + suffix;
#else
        return string.Concat(input.AsSpan(0, maxLength - suffix.Length), suffix);
#endif
    }

    /// <summary>
    ///     Counts occurrences of a substring (case-sensitive).
    /// </summary>
    /// <param name="substring">The substring to search for occurrences of.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The number of occurrences of the substring in the string.</returns>
    public static int CountOccurrences(this string value, string substring)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(substring))
        {
            return 0;
        }

        var count = 0;
        var index = 0;

        while ((index = value.IndexOf(substring, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += substring.Length;
        }

        return count;
    }

    /// <summary>
    ///     Replaces multiple spaces with a single space and trims
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <returns>The string with normalized whitespace.</returns>
    public static string NormalizeWhitespace(this string value)
    {
        return string.IsNullOrEmpty(value) ? value : Regex.Replace(value.Trim(), @"\s+", " ");
    }

    /// <summary>
    ///     Converts the string into a "slug," which is an identifier that is also somewhat readable by people.
    /// </summary>
    /// <param name="value">The string to act upon.</param>
    /// <remarks>
    ///     These are often used in place of long universally unique identifiers when there will some human interaction with
    ///     the values.
    /// </remarks>
    /// <returns>A kebab and lower case conversion of the string with no more than <see cref="MaxSlugLength" /> characters.</returns>
    public static string ToSlug(this string value)
    {
        return value.ToLowerInvariant().ToKebabCase().Truncate(MaxSlugLength);
    }

    /// <summary>
    ///     Returns whether the string contains any of the values provided.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool ContainsAny(this string value, params string[] values)
    {
        return values.Any(value.Contains);
    }

    /// <summary>
    ///     Returns whether the string contains any of the values provided. Case insensitive.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool NoCaseContainsAny(this string value, params string[] values)
    {
        return values.Any(val => value.Contains(val, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    ///     Returns whether the string contains all the values provided.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool ContainsAll(this string value, params string[] values)
    {
        return values.All(value.Contains);
    }

    /// <summary>
    ///     Returns whether the string contains all the values provided. Case insensitive.
    /// </summary>
    /// <param name="values">The values to check for in the string.</param>
    /// <param name="value">The string to act upon.</param>
    /// <returns>True or false.</returns>
    public static bool NoCaseContainsAll(this string value, params string[] values)
    {
        return values.All(val => value.Contains(val, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc cref="StringBuilder.AppendLine()" />
    /// <remarks>
    ///     This and similar methods are part of a suite of helper methods that automatically account for .NET SDK style
    ///     libraries that must accomodate early methods and newer methods that must provide a format provider, defaulting to
    ///     <see cref="CultureInfo.CurrentCulture" /> where possible.
    /// </remarks>
    public static StringBuilder AppendWith(this StringBuilder stringBuilder)
    {
        return stringBuilder.AppendLine();
    }

    /// <inheritdoc cref="StringBuilder.AppendLine(string)" />
    /// <remarks>
    ///     This and similar methods are part of a suite of helper methods that automatically account for .NET SDK style
    ///     libraries that must accomodate early methods and newer methods that must provide a format provider, defaulting to
    ///     <see cref="CultureInfo.CurrentCulture" /> where possible.
    /// </remarks>
    public static StringBuilder AppendWith(this StringBuilder stringBuilder, string? value)
    {
        return stringBuilder.AppendLine(value);
    }

    /// <inheritdoc cref="StringBuilder.AppendLine(string)" />
    /// <remarks>
    ///     This and similar methods are part of a suite of helper methods that automatically account for .NET SDK style
    ///     libraries that must accomodate early methods and newer methods that must provide a format provider, defaulting to
    ///     <see cref="CultureInfo.CurrentCulture" /> where possible.
    /// </remarks>
    public static StringBuilder AppendWith(this StringBuilder stringBuilder, FormattableString formattable,
        CultureInfo? culture = null)
    {
        return stringBuilder.AppendLine(formattable.ToString(culture ?? CultureInfo.CurrentCulture));
    }
}
