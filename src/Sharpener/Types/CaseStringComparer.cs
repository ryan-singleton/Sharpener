using Sharpener.Types.Interfaces;

namespace Sharpener.Types;

/// <inheritdoc />
public class CaseStringComparer : IStringComparer
{
    /// <inheritdoc />
    public string Source { get; }

    /// <summary>
    /// Whether to ignore case or not.
    /// </summary>
    /// <value></value>
    public bool Ignore { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="ignore"></param>
    public CaseStringComparer(string source, bool ignore)
    {
        Source = source;
        Ignore = ignore;
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
    ? Source.Equals(compare, StringComparison.OrdinalIgnoreCase)
    : Source.Equals(compare, StringComparison.Ordinal);

    /// <inheritdoc />
    public bool Contains(string compare) => Ignore
    ? Source.Contains(compare, StringComparison.OrdinalIgnoreCase)
    : Source.Contains(compare, StringComparison.Ordinal);

    /// <inheritdoc />
    public bool StartsWith(string compare) => Ignore
    ? Source.StartsWith(compare, StringComparison.OrdinalIgnoreCase)
    : Source.StartsWith(compare, StringComparison.Ordinal);

    /// <inheritdoc />
    public bool EndsWith(string compare) => Ignore
    ? Source.EndsWith(compare, StringComparison.OrdinalIgnoreCase)
    : Source.EndsWith(compare, StringComparison.Ordinal);
}
