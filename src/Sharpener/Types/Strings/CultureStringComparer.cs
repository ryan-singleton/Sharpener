using Sharpener.Types.Strings.Interfaces;

namespace Sharpener.Types.Strings;

/// <inheritdoc />
internal struct CultureStringComparer : IStringComparer
{
    /// <inheritdoc />
    public string Source { get; }

    /// <summary>
    /// The culture.
    /// </summary>
    /// <value></value>
    internal StringComparison Comparison { get; private set; }

    /// <summary>
    /// Constructor.
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
        StringComparison newComparison = Comparison switch
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
        StringComparison newComparison = Comparison switch
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
        StringComparison newComparison = Comparison switch
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
        StringComparison newComparison = Comparison switch
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
        StringComparison newComparison = Comparison switch
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
    public bool Equals(string compare) => Source.Equals(compare, Comparison);

    /// <inheritdoc />
    public bool Contains(string compare) => Source.Contains(compare, Comparison);

    /// <inheritdoc />
    public bool EndsWith(string compare) => Source.EndsWith(compare, Comparison);

    /// <inheritdoc />
    public bool StartsWith(string compare) => Source.StartsWith(compare, Comparison);
}
