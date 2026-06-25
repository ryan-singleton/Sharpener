// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.NamedResources;

/// <summary>
///     The makeup of a named resource type, so that we can statically define common named values and refer to them with
///     composition and generic types.
/// </summary>
public interface INamedResource
{
    /// <summary>
    ///     Gets the resource name.
    /// </summary>
    public string Name { get; }
}
