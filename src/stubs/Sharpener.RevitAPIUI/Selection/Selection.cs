// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.UI.Selection;

public class Selection
{
    public IEnumerable<object> GetElementIds()
    {
        return [];
    }

    public SelectedRef PickObjects(object objectType, ISelectionFilter filter, string? prompt)
    {
        return new SelectedRef();
    }
}
