// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Types.Strings;

using Interfaces;

/// <inheritdoc />
internal struct CaseStringComparer : IStringComparer
{
    /// <inheritdoc />
    public string Source { get; }

    /// <summary>
    ///     Whether to ignore case or not.
    /// </summary>
    /// <value></value>
    private bool Ignore { get; set; }

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="ignore"></param>
    internal CaseStringComparer(string source, bool ignore)
    {
        Source = source;
        Ignore = ignore;
    }

    /// <inheritdoc />
    public bool Equals(IStringComparer comparer)
    {
        return comparer.Equals(Source);
    }

    /// <inheritdoc />
    public IStringComparer NoCase()
    {
        Ignore = true;
        return this;
    }

    /// <inheritdoc />
    public IStringComparer Case()
    {
        Ignore = false;
        return this;
    }

    /// <inheritdoc />
    public IStringComparer Current()
    {
        return Ignore
            ? new CultureStringComparer(Source, StringComparison.CurrentCultureIgnoreCase)
            : new CultureStringComparer(Source, StringComparison.CurrentCulture);
    }

    /// <inheritdoc />
    public IStringComparer Ordinal()
    {
        return Ignore
            ? new CultureStringComparer(Source, StringComparison.OrdinalIgnoreCase)
            : new CultureStringComparer(Source, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public IStringComparer Invariant()
    {
        return Ignore
            ? new CultureStringComparer(Source, StringComparison.InvariantCultureIgnoreCase)
            : new CultureStringComparer(Source, StringComparison.InvariantCulture);
    }

    /// <inheritdoc />
    public bool Equals(string compare)
    {
        return Ignore
            ? Source.Equals(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
            : Source.Equals(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);
    }

    /// <inheritdoc />
    public bool Contains(string compare)
    {
        # if NET5_0_OR_GREATER
        return Ignore
            ? Source.Contains(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
            : Source.Contains(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);
        #endif
        #if NETSTANDARD2_0_OR_GREATER
        return Ignore
            ? Source.IndexOf(compare, StringComparison.CurrentCultureIgnoreCase) >= 0
            : Source.IndexOf(compare, StringComparison.CurrentCulture) >= 0;
        #endif
    }

    /// <inheritdoc />
    public bool StartsWith(string compare)
    {
        return Ignore
            ? Source.StartsWith(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
            : Source.StartsWith(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);
    }

    /// <inheritdoc />
    public bool EndsWith(string compare)
    {
        return Ignore
            ? Source.EndsWith(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
            : Source.EndsWith(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);
    }
}
