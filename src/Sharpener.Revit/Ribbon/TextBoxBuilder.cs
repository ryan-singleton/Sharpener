// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;
using TextBox = Autodesk.Revit.UI.TextBox;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a text box in a Revit ribbon panel.
/// </summary>
public class TextBoxBuilder(string name, string prompt = "") : IRibbonElementBuilder
{
    /// <summary>
    ///     Builds a text box element and adds it to the specified Revit ribbon panel.
    /// </summary>
    /// <param name="panel">The ribbon panel where the text box will be added.</param>
    public void Build(RibbonPanel panel)
    {
        var textBoxData = new TextBoxData(name);
        var textBox = (TextBox)panel.AddItem(textBoxData);
        textBox.PromptText = prompt;
    }
}
