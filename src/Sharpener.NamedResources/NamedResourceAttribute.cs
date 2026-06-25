// The Sharpener project licenses this file to you under the MIT license.

// ReSharper disable UnusedParameter.Local

namespace Sharpener.NamedResources;

/// <summary>
///     Marks an enum as a named resource source. The source generator will emit
///     one <c>readonly struct</c> per member implementing the named resource type,
///     and a static class of string constants for each member.
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public sealed class NamedResourceAttribute : Attribute
{
    /// <param name="namedResourceType">
    ///     The type from which the generated resource types will derive. This must implement
    ///     <see cref="INamedResource" />.
    /// </param>
    /// <param name="constantsClassName">
    ///     Name of the generated static class holding string constants. Optional. Will be generated if not provided, derived
    ///     from <paramref name="namedResourceType" />.
    /// </param>
    /// <param name="namedResourceNamespace">
    ///     Namespace under which the generated types will be placed. Optional. Will be
    ///     generated if not provided, derived from <paramref name="namedResourceType" /> namespace.
    /// </param>
    /// <param name="constantsClassNamespace">
    ///     Namespace under which the constants will be placed. Optional. Will be generated
    ///     if not provided, derived from <paramref name="namedResourceType" /> namespace.
    /// </param>
    /// <param name="constantsClassSummary">
    ///     The summary for intellisense regarding the constants class. Optional. Will be
    ///     generated if not provided, derived from <paramref name="namedResourceType" />.
    /// </param>
    public NamedResourceAttribute(Type namedResourceType, string? constantsClassName = null,
        string? namedResourceNamespace = null,
        string? constantsClassNamespace = null, string? constantsClassSummary = null)
    {
    }
}
