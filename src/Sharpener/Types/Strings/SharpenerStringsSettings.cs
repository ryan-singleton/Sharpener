// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Extensions;

namespace Sharpener.Types.Strings;

/// <summary>
///     Global settings for all Sharpener strings features.
/// </summary>
public struct SharpenerStringsSettings
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    static SharpenerStringsSettings()
    {
        SetDefaultCulture(StringComparison.Ordinal);
    }

    /// <summary>
    ///     The default fallback value for a string when calling <see cref="StringExtensions.OrFallback" />.
    /// </summary>
    public static string DefaultFallback { get; set; } = "null";

    /// <summary>
    ///     The default culture for case sensitive comparison.
    /// </summary>
    /// Note that the default for this already matches
    public static StringComparison DefaultCultureCaseSensitive { get; private set; }

    /// <summary>
    ///     The default culture for case insensitive comparison.
    /// </summary>
    public static StringComparison DefaultCultureCaseInsensitive { get; private set; }

    /// <summary>
    ///     Sets the default culture for case insensitive comparison. Will return false if the parameter is not case
    ///     insensitive.
    /// </summary>
    /// <param name="stringComparison">The case insensitive value to set it to.</param>
    /// <returns></returns>
    public static bool TrySetDefaultCultureCaseInsensitive(StringComparison stringComparison)
    {
        if (stringComparison is not (StringComparison.CurrentCultureIgnoreCase
            or StringComparison.InvariantCultureIgnoreCase or StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        DefaultCultureCaseInsensitive = stringComparison;
        return true;
    }

    /// <summary>
    ///     /// Sets the default culture for case sensitive comparison. Will return false if the parameter is not case
    ///     sensitive.
    /// </summary>
    /// <param name="stringComparison">The case sensitive value to set it to.</param>
    /// <returns></returns>
    public static bool TrySetDefaultCultureCaseSensitive(StringComparison stringComparison)
    {
        if (stringComparison is not (StringComparison.CurrentCulture or StringComparison.InvariantCulture
            or StringComparison.Ordinal))
        {
            return false;
        }

        DefaultCultureCaseSensitive = stringComparison;
        return true;
    }

    /// <summary>
    ///     Sets the default culture for comparison. Sets both case sensitivities keying off of the parameter.
    /// </summary>
    /// <param name="stringComparison">The culture to use.</param>
    private static void SetDefaultCulture(StringComparison stringComparison)
    {
        switch (stringComparison)
        {
            case StringComparison.CurrentCulture:
            case StringComparison.CurrentCultureIgnoreCase:
                DefaultCultureCaseInsensitive = StringComparison.CurrentCultureIgnoreCase;
                DefaultCultureCaseSensitive = StringComparison.CurrentCulture;
                break;
            case StringComparison.Ordinal:
            case StringComparison.OrdinalIgnoreCase:
                DefaultCultureCaseInsensitive = StringComparison.OrdinalIgnoreCase;
                DefaultCultureCaseSensitive = StringComparison.Ordinal;
                break;
            case StringComparison.InvariantCulture:
            case StringComparison.InvariantCultureIgnoreCase:
                DefaultCultureCaseInsensitive = StringComparison.InvariantCultureIgnoreCase;
                DefaultCultureCaseSensitive = StringComparison.InvariantCulture;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stringComparison), stringComparison, null);
        }
    }
}
