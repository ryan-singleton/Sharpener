using Sharpener.Types.Interfaces;

namespace Sharpener.Types;

/// <inheritdoc />
internal sealed class CultureStringComparer : IStringComparer
{
    /// <inheritdoc />
    internal string Source { get; }

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
    internal IStringComparer NoCase()
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
    internal IStringComparer Case()
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
    internal IStringComparer Current()
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
    internal IStringComparer Invariant()
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
    internal IStringComparer Ordinal()
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
    internal bool Equals(string compare) => Source.Equals(compare, Comparison);

    /// <inheritdoc />
    internal bool Contains(string compare) => Source.Contains(compare, Comparison);

    /// <inheritdoc />
    internal bool EndsWith(string compare) => Source.EndsWith(compare, Comparison);

    /// <inheritdoc />
    internal bool StartsWith(string compare) => Source.StartsWith(compare, Comparison);
}
