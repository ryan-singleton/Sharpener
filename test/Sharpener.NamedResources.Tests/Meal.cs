// The Sharpener project licenses this file to you under the MIT license.

using System.ComponentModel;

namespace Sharpener.NamedResources.Tests;

/// <summary>
/// Types of meals.
/// </summary>
[NamedResource(typeof(IMealName), "Food")]
public enum Meal
{
    /// <summary>
    /// A bown of cereal.
    /// </summary>
    [Description("Cereal Bowl")]
    CerealBowl,
    /// <summary>
    /// A cut of ribeye steak.
    /// </summary>
    [Description("Ribeye Steak")]
    RibeyeSteak,
}
