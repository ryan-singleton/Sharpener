// The Sharpener project and Facefire license this file to you under the MIT license.

namespace Sharpener.Tests.Common.Models;

/// <summary>
/// A simple model used for testing against.
/// </summary>
public class Item
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="description">Description</param>
    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// The name.
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// The description.
    /// </summary>
    /// <value></value>
    public string Description { get; set; }
}
