// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a Revit ribbon separator.
/// </summary>
public class SeparatorBuilder : IRibbonElementBuilder
{
    /// <summary>
    ///     Adds a separator to the specified Revit ribbon panel.
    /// </summary>
    /// <param name="panel">The ribbon panel to which the separator is added.</param>
    public void Build(RibbonPanel panel)
    {
        panel.AddSeparator();
    }
}
