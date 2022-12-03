using Sharpener.Types.Interfaces;

namespace Sharpener.Types;

/// <inheritdoc />
internal sealed class CaseStringComparer : IStringComparer
{
    /// <inheritdoc />
    internal string Source { get; }

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
    internal IStringComparer NoCase()
    {
        Ignore = true;
        return this;
    }

    /// <inheritdoc />
    internal IStringComparer Case()
    {
        Ignore = false;
        return this;
    }

    /// <inheritdoc />
    internal IStringComparer Current() => Ignore
        ? new CultureStringComparer(Source, StringComparison.CurrentCultureIgnoreCase)
        : new CultureStringComparer(Source, StringComparison.CurrentCulture);

    /// <inheritdoc />
    internal IStringComparer Ordinal() => Ignore
        ? new CultureStringComparer(Source, StringComparison.OrdinalIgnoreCase)
        : new CultureStringComparer(Source, StringComparison.Ordinal);

    /// <inheritdoc />
    internal IStringComparer Invariant() => Ignore
        ? new CultureStringComparer(Source, StringComparison.InvariantCultureIgnoreCase)
        : new CultureStringComparer(Source, StringComparison.InvariantCulture);

    /// <inheritdoc />
    internal bool Equals(string compare) => Ignore
        ? Source.Equals(compare, StringComparison.OrdinalIgnoreCase)
        : Source.Equals(compare, StringComparison.Ordinal);

    /// <inheritdoc />
    internal bool Contains(string compare) => Ignore
        ? Source.Contains(compare, StringComparison.OrdinalIgnoreCase)
        : Source.Contains(compare, StringComparison.Ordinal);

    /// <inheritdoc />
    internal bool StartsWith(string compare) => Ignore
        ? Source.StartsWith(compare, StringComparison.OrdinalIgnoreCase)
        : Source.StartsWith(compare, StringComparison.Ordinal);

    /// <inheritdoc />
    internal bool EndsWith(string compare) => Ignore
        ? Source.EndsWith(compare, StringComparison.OrdinalIgnoreCase)
        : Source.EndsWith(compare, StringComparison.Ordinal);
}
