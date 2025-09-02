// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;
using Sharpener.Revit.Extensions;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a PushButton in a Revit ribbon panel.
/// </summary>
/// <typeparam name="T">The type of the command to be executed when the button is clicked.</typeparam>
public class PushButtonBuilder<T> : IRibbonElementBuilder, IPushButtonBuilder, IHasIcon
{
    private readonly string? _assembly = typeof(T).Assembly.Location, _className = typeof(T).FullName!;
    private readonly string? _name, _text;
    private string? _tooltip;

    /// <summary>
    ///     Creates a new builder for PushButtons in a Revit ribbon  panel.
    /// </summary>
    /// <param name="name">The name of the push button.</param>
    /// <param name="text">The text in the push button.</param>
    public PushButtonBuilder(string? name, string? text)
    {
        _name = name;
        _text = text;
    }

    /// <inheritdoc />
    public string? IconPath { get; set; }

    /// <inheritdoc />
    public PushButtonData ToPushButtonData()
    {
        var pushButtonData = new PushButtonData(_name, _text, _assembly, _className);
        if (!string.IsNullOrWhiteSpace(_tooltip))
        {
            pushButtonData.ToolTip = _tooltip;
        }

        pushButtonData.LargeImage = this.GetIcon();
        return pushButtonData;
    }

    /// <inheritdoc />
    public void Build(RibbonPanel panel)
    {
        var data = ToPushButtonData();

        panel.AddItem(data);
    }

    /// <summary>
    ///     Specifies the icons to be used for the PushButton being built.
    /// </summary>
    /// <param name="paths">An array of strings representing the file paths to the icon images.</param>
    /// <returns>The current instance of <see cref="PushButtonBuilder{T}" /> with updated icon paths.</returns>
    public PushButtonBuilder<T> WithIcon(params string[] paths)
    {
        return RibbonExtensions.WithIcon(this, paths);
    }

    /// <summary>
    ///     Sets the tooltip text for the PushButton being built.
    /// </summary>
    /// <param name="tooltip">The text to be displayed as the tooltip for the PushButton.</param>
    /// <returns>The current instance of <see cref="PushButtonBuilder{T}" /> with the updated tooltip.</returns>
    public PushButtonBuilder<T> WithTooltip(string tooltip)
    {
        _tooltip = tooltip;
        return this;
    }
}
