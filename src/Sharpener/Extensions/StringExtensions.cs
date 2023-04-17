// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Extensions;

using Types.Strings;
using Types.Strings.Interfaces;

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
}
