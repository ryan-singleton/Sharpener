// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Defines an interface for creating and building an element in a Revit ribbon panel.
/// </summary>
public interface IRibbonElementBuilder
{
    /// <summary>
    ///     Builds an element and adds it to the specified Revit ribbon panel.
    /// </summary>
    /// <param name="panel">The ribbon panel to which the element will be added.</param>
    void Build(RibbonPanel panel);
}
