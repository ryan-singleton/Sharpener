// The Sharpener project licenses this file to you under the MIT license.

using System.ComponentModel;

namespace Sharpener.NamedResources.Tests;

[NamedResource(typeof(IIngredientName))]
public enum Ingredient
{
    /// <summary>A tart green apple variety.</summary>
    [Description("Granny Smith Apple")] GrannySmith,

    [Description("Unsalted Butter")] Butter
}
