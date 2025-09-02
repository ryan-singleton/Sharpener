// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Sharpener.Revit.Selection;

/// <summary>
///     Selection filter to only allow specific derivatives of <see cref="Autodesk.Revit.DB.Element" /> in a selection.
/// </summary>
public class ElementTypeSelectionFilter<T> : ISelectionFilter where T : Element
{
    /// <inheritdoc />
    public bool AllowElement(Element elem)
    {
        return elem is T;
    }

    /// <inheritdoc />
    public bool AllowReference(Reference reference, XYZ position)
    {
        return false;
    }
}
