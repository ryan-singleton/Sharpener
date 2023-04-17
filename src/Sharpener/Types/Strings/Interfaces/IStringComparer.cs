// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Types.Strings.Interfaces;

/// <summary>
///     A pairing of a string and o
/// </summary>
public interface IStringComparer
{
    /// <summary>
    ///     The original string for which comparisons are to take place.
    /// </summary>
    /// <value></value>
    string Source { get; }

    /// <summary>
    ///     Changes the Culture to the case sensitive variant for the eventual comparison.
    /// </summary>
    /// <returns></returns>
    IStringComparer Case();

    /// <summary>
    ///     Performs a contains check with the comparison parameters provided.
    /// </summary>
    /// <param name="compare"></param>
    /// <returns></returns>
    bool Contains(string compare);

    /// <summary>
    ///     Changes the Culture to CurrentCulture for the eventual comparison.
    /// </summary>
    /// <returns></returns>
    IStringComparer Current();

    /// <summary>
    ///     Performs an ends with check with the comparison parameters provided.
    /// </summary>
    /// <param name="compare"></param>
    /// <returns></returns>
    bool EndsWith(string compare);

    /// <summary>
    ///     Tests equality against another comparer.
    /// </summary>
    /// <param name="comparer">The other comparer.</param>
    /// <returns></returns>
    bool Equals(IStringComparer comparer);

    /// <summary>
    ///     Performs an equality check with the comparison parameters provided.
    /// </summary>
    /// <param name="compare"></param>
    /// <returns></returns>
    bool Equals(string compare);

    /// <summary>
    ///     Changes the Culture to Invariant for the eventual comparison.
    /// </summary>
    /// <returns></returns>
    IStringComparer Invariant();

    /// <summary>
    ///     Changes the Culture to the case insensitive variant for the eventual comparison.
    /// </summary>
    /// <returns></returns>
    IStringComparer NoCase();

    /// <summary>
    ///     Changes the Culture to Ordinal for the eventual comparison.
    /// </summary>
    /// <returns></returns>
    IStringComparer Ordinal();

    /// <summary>
    ///     Performs a starts with check with the comparison parameters provided.
    /// </summary>
    /// <param name="compare"></param>
    /// <returns></returns>
    bool StartsWith(string compare);
}
