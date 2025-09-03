// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.UI;

// ReSharper disable once InconsistentNaming
public class UIDocument
{
    public Selection.Selection Selection { get; set; } = null!;
    public Document Document { get; set; } = null!;
}
