// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Types.Strings.Interfaces;

namespace Sharpener.Types.Strings;

/// <inheritdoc />
internal struct CultureStringComparer : IStringComparer
{
    /// <inheritdoc />
    public string Source { get; }

    /// <summary>
    ///     The culture.
    /// </summary>
    /// <value></value>
    private StringComparison Comparison { get; set; }

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="comparison"></param>
    internal CultureStringComparer(string source, StringComparison comparison)
    {
        Source = source;
        Comparison = comparison;
    }

    /// <inheritdoc />
    public bool Equals(IStringComparer comparer)
    {
        return comparer.Equals(Source);
    }

    /// <inheritdoc />
    public IStringComparer NoCase()
    {
        var newComparison = Comparison switch
        {
            StringComparison.CurrentCulture => StringComparison.CurrentCultureIgnoreCase,
            StringComparison.Ordinal => StringComparison.OrdinalIgnoreCase,
            StringComparison.InvariantCulture => StringComparison.InvariantCultureIgnoreCase,
            _ => Comparison
        };

        Comparison = newComparison;
        return this;
    }

    /// <inheritdoc />
    public IStringComparer Case()
    {
        var newComparison = Comparison switch
        {
            StringComparison.CurrentCultureIgnoreCase => StringComparison.CurrentCulture,
            StringComparison.OrdinalIgnoreCase => StringComparison.Ordinal,
            StringComparison.InvariantCultureIgnoreCase => StringComparison.InvariantCulture,
            _ => Comparison
        };

        Comparison = newComparison;
        return this;
    }

    /// <inheritdoc />
    public IStringComparer Current()
    {
        var newComparison = Comparison switch
        {
            StringComparison.Ordinal => StringComparison.CurrentCulture,
            StringComparison.InvariantCulture => StringComparison.CurrentCulture,
            StringComparison.OrdinalIgnoreCase => StringComparison.CurrentCultureIgnoreCase,
            StringComparison.InvariantCultureIgnoreCase => StringComparison.CurrentCultureIgnoreCase,

            _ => Comparison
        };

        Comparison = newComparison;
        return this;
    }

    /// <inheritdoc />
    public IStringComparer Invariant()
    {
        var newComparison = Comparison switch
        {
            StringComparison.Ordinal => StringComparison.InvariantCulture,
            StringComparison.CurrentCulture => StringComparison.InvariantCulture,
            StringComparison.OrdinalIgnoreCase => StringComparison.InvariantCultureIgnoreCase,
            StringComparison.CurrentCultureIgnoreCase => StringComparison.InvariantCultureIgnoreCase,

            _ => Comparison
        };

        Comparison = newComparison;
        return this;
    }

    /// <inheritdoc />
    public IStringComparer Ordinal()
    {
        var newComparison = Comparison switch
        {
            StringComparison.InvariantCulture => StringComparison.Ordinal,
            StringComparison.CurrentCulture => StringComparison.Ordinal,
            StringComparison.InvariantCultureIgnoreCase => StringComparison.OrdinalIgnoreCase,
            StringComparison.CurrentCultureIgnoreCase => StringComparison.OrdinalIgnoreCase,

            _ => Comparison
        };

        Comparison = newComparison;
        return this;
    }

    /// <inheritdoc />
    public bool Equals(string compare)
    {
        return Source.Equals(compare, Comparison);
    }

    /// <inheritdoc />
    public bool Contains(string compare)
    {
#if NET5_0_OR_GREATER
        return Source.Contains(compare, Comparison);
#else
        return Source.IndexOf(compare, Comparison) >= 0;
#endif
    }

    /// <inheritdoc />
    public bool EndsWith(string compare)
    {
        return Source.EndsWith(compare, Comparison);
    }

    /// <inheritdoc />
    public bool StartsWith(string compare)
    {
        return Source.StartsWith(compare, Comparison);
    }
}
