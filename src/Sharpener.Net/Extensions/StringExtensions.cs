using System.Globalization;
using Sharpener.Net.Preferences;

namespace Sharpener.Net.Extensions;

/// <summary>
/// Extensions for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Prepares a string comparison operation with case insensitivity. Expects to call methods like those in the remarks afterward.
    /// </summary>
    /// <remarks>
    /// Continue building with Current(), Ordinal(), or Invariant().
    /// Finalize up with Is(), Has(), Starts(), Ends(), etc.
    /// </remarks>
    /// <returns></returns>
    public static (bool Ignore, string Source) NoCase(this string source) => (true, source);

    /// <summary>
    /// Prepares a string comparison operation with case sensitivity. You can skip this, however, as case sensitivity is the default. Expects to call methods like those in the remarks afterward.
    /// </summary>
    /// <remarks>
    /// Continue building with Current(), Ordinal(), or Invariant().
    /// Finalize up with Is(), Has(), Starts(), Ends(), etc. (Can skip to these with case insensitivity).
    /// </remarks>
    /// <returns></returns>
    public static (bool Ignore, string Source) Case(this string source) => (false, source);

    /// <summary>
    /// Prepares the string comparison with current culture treatment.
    /// </summary>
    /// <returns></returns>
    public static (StringComparison Comparison, string Source) Current(this string source) => (StringComparison.CurrentCulture, source);

    /// <summary>
    /// Prepares the string comparison with invariant culture treatment.
    /// </summary>
    /// <returns></returns>
    public static (StringComparison Comparison, string Source) Invariant(this string source) => (StringComparison.InvariantCulture, source);

    /// <summary>
    /// Prepares the string comparison with ordinal culture treatment.
    /// </summary>
    /// <returns></returns>
    public static (StringComparison Comparison, string Source) Ordinal(this string source) => (StringComparison.Ordinal, source);

    /// <summary>
    /// Prepares the string comparison with current culture treatment.
    /// </summary>
    /// <returns></returns>
    public static (StringComparison Comparison, string Source) Current(this (bool Ignore, string Source) pair) => pair.Ignore
        ? (StringComparison.CurrentCultureIgnoreCase, pair.Source)
        : (StringComparison.CurrentCulture, pair.Source);

    /// <summary>
    /// Prepares the string comparison with invariant culture treatment.
    /// </summary>
    /// <returns></returns>
    public static (StringComparison Comparison, string Source) Invariant(this (bool Ignore, string Source) pair) => pair.Ignore
        ? (StringComparison.CurrentCultureIgnoreCase, pair.Source)
        : (StringComparison.CurrentCulture, pair.Source);

    /// <summary>
    /// Prepares the string comparison with ordinal culture treatment.
    /// </summary>
    /// <returns></returns>
    public static (StringComparison Comparison, string Source) Ordinal(this (bool Ignore, string Source) pair) => pair.Ignore
        ? (StringComparison.CurrentCultureIgnoreCase, pair.Source)
        : (StringComparison.CurrentCulture, pair.Source);

    /// <summary>
    /// Performs an equality check based on the comparison rules received.
    /// </summary>
    /// <param name="source">The string to compare.</param>
    /// <param name="compare">The string to compare to.</param>
    /// <returns></returns>
    public static bool Is(this (StringComparison Comparison, string Source) pair, string compare) => pair.Source.Equals(compare, pair.Comparison);

    /// <summary>
    /// Performs an equality check with ordinal and case sensitivity based on the ignore value received.
    /// </summary>
    /// <param name="source">The string to compare.</param>
    /// <param name="compare">The string to compare to.</param>
    /// <returns></returns>
    public static bool Is(this (bool Ignore, string Source) pair, string compare) => pair.Source.Equals(compare, GetDefaultComparison(pair.Ignore));

    /// <summary>
    /// Performs an equality check with ordinal case sensitive comparison rules.
    /// </summary>
    /// <param name="source">The string to compare.</param>
    /// <param name="compare">The string to compare to.</param>
    /// <returns></returns>
    public static bool Is(this string source, string compare) => source.Equals(compare, _default);

    /// <summary>
    /// Performs a contains check based on the comparison rules received.
    /// </summary>
    /// <param name="source">The string to compare.</param>
    /// <param name="compare">The string to compare to.</param>
    /// <returns></returns>
    public static bool Has(this (StringComparison Comparison, string Source) pair, string compare) => pair.Source.Contains(compare, pair.Comparison);

    /// <summary>
    /// Performs a contains check with ordinal and case sensitivity based on the ignore value received.
    /// </summary>
    /// <param name="source">The string to compare.</param>
    /// <param name="compare">The string to compare to.</param>
    /// <returns></returns>
    public static bool Has(this (bool Ignore, string Source) pair, string compare) => pair.Source.Contains(compare, GetDefaultComparison(pair.Ignore));

    /// <summary>
    /// Performs a contains check with ordinal case sensitive comparison rules.
    /// </summary>
    /// <param name="source">The string to compare.</param>
    /// <param name="compare">The string to compare to.</param>
    /// <returns></returns>
    public static bool Has(this string source, string compare) => source.Contains(compare, _default);


    private static StringComparison _defaultNoCase = StringComparison.OrdinalIgnoreCase;

    private static StringComparison _defaultWithCase = StringComparison.Ordinal;

    private static StringComparison _default = _defaultWithCase;

    private static StringComparison GetDefaultComparison(bool ignore) => ignore ? _defaultNoCase : _defaultWithCase;


}