// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Types.Strings.Interfaces;

namespace Sharpener.Types.Strings;

/// <inheritdoc />
internal struct CaseStringComparer : IStringComparer
{
    /// <inheritdoc />
    public string Source { get; }

    /// <summary>
    /// Whether to ignore case or not.
    /// </summary>
    /// <value></value>
    internal bool Ignore { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="ignore"></param>
    internal CaseStringComparer(string source, bool ignore)
    {
        Source = source;
        Ignore = ignore;
    }

    /// <inheritdoc />
    public bool Equals(IStringComparer comparer) => comparer.Equals(Source);

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
    public IStringComparer Current() => Ignore
        ? new CultureStringComparer(Source, StringComparison.CurrentCultureIgnoreCase)
        : new CultureStringComparer(Source, StringComparison.CurrentCulture);

    /// <inheritdoc />
    public IStringComparer Ordinal() => Ignore
        ? new CultureStringComparer(Source, StringComparison.OrdinalIgnoreCase)
        : new CultureStringComparer(Source, StringComparison.Ordinal);

    /// <inheritdoc />
    public IStringComparer Invariant() => Ignore
        ? new CultureStringComparer(Source, StringComparison.InvariantCultureIgnoreCase)
        : new CultureStringComparer(Source, StringComparison.InvariantCulture);

    /// <inheritdoc />
    public bool Equals(string compare) => Ignore
        ? Source.Equals(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
        : Source.Equals(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);

    /// <inheritdoc />
    public bool Contains(string compare) => Ignore
        ? Source.Contains(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
        : Source.Contains(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);

    /// <inheritdoc />
    public bool StartsWith(string compare) => Ignore
        ? Source.StartsWith(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
        : Source.StartsWith(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);

    /// <inheritdoc />
    public bool EndsWith(string compare) => Ignore
        ? Source.EndsWith(compare, SharpenerStringsSettings.DefaultCultureCaseInsensitive)
        : Source.EndsWith(compare, SharpenerStringsSettings.DefaultCultureCaseSensitive);
}
