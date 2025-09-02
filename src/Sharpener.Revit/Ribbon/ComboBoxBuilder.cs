// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;
using ComboBox = Autodesk.Revit.UI.ComboBox;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a ComboBox in a Revit ribbon panel.
/// </summary>
/// <remarks>
///     A class that provides methods for building and configuring a ComboBox element
///     to be added to a Revit ribbon panel.
/// </remarks>
public class ComboBoxBuilder(string name) : IRibbonElementBuilder
{
    private readonly List<ComboBoxMemberData> _items = [];
    private readonly string _name = name;

    /// <summary>
    ///     Builds a ComboBox element and adds it to the specified Revit ribbon panel.
    /// </summary>
    /// <param name="panel">The ribbon panel to which the ComboBox will be added.</param>
    public void Build(RibbonPanel panel)
    {
        var comboBoxData = new ComboBoxData(_name);
        var comboBox = (ComboBox)panel.AddItem(comboBoxData);

        foreach (var memberData in _items)
        {
            comboBox.AddItem(memberData);
        }
    }

    /// <summary>
    ///     Adds an item to the ComboBox being built.
    /// </summary>
    /// <param name="name">The unique name of the item to be added to the ComboBox.</param>
    /// <param name="text">The display text for the item to be added.</param>
    /// <returns>Returns the current instance of <see cref="ComboBoxBuilder" />, allowing for method chaining.</returns>
    public ComboBoxBuilder AddItem(string name, string text)
    {
        _items.Add(new ComboBoxMemberData(name, text));
        return this;
    }
}
