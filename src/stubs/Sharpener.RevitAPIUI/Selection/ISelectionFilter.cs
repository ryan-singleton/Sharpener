// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.DB;

namespace Autodesk.Revit.UI.Selection;

public interface ISelectionFilter
{
    public bool AllowElement(Element elem);
    public bool AllowReference(Reference reference, XYZ position);
}
