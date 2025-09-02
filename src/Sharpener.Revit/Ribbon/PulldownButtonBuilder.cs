// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;
using Sharpener.Revit.Extensions;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a PulldownButton in a Revit ribbon panel.
/// </summary>
public class PulldownButtonBuilder : IRibbonElementBuilder, IHasIcon
{
    private readonly List<IPushButtonBuilder> _buttonBuilders = [];
    private readonly string _name, _text;
    private string? _tooltip;

    /// <summary>
    ///     A class that provides methods for building and configuring a PulldownButton element
    ///     to be added to a Revit ribbon panel.
    /// </summary>
    public PulldownButtonBuilder(string name, string text)
    {
        _name = name;
        _text = text;
    }

    /// <inheritdoc />
    public string? IconPath { get; set; }

    /// <inheritdoc />
    public void Build(RibbonPanel panel)
    {
        var pulldownButtonData = new PulldownButtonData(_name, _text);
        if (!string.IsNullOrWhiteSpace(_tooltip))
        {
            pulldownButtonData.ToolTip = _tooltip;
        }

        pulldownButtonData.LargeImage = this.GetIcon();

        var pulldownButton = (PulldownButton)panel.AddItem(pulldownButtonData);
        foreach (var buttonBuilder in _buttonBuilders)
        {
            pulldownButton.AddPushButton(buttonBuilder.ToPushButtonData());
        }
    }

    /// <summary>
    ///     Adds a PushButton to the PulldownButton being built.
    /// </summary>
    /// <param name="name">The unique name of the PushButton to be added to the PulldownButton.</param>
    /// <param name="text">The text to be displayed on the PushButton.</param>
    /// <param name="config">An optional action to configure the PushButton being added.</param>
    /// <returns>The PulldownButtonBuilder instance.</returns>
    public PulldownButtonBuilder AddPushButton<T>(string name, string text, Action<PushButtonBuilder<T>>? config)
    {
        var pushButtonBuilder = new PushButtonBuilder<T>(name, text);
        config?.Invoke(pushButtonBuilder);
        _buttonBuilders.Add(pushButtonBuilder);
        return this;
    }

    /// <summary>
    ///     Configures the PulldownButtonBuilder to use the specified icon file paths.
    /// </summary>
    /// <param name="paths">An array of string paths pointing to the icon files to be used.</param>
    /// <returns>The PulldownButtonBuilder instance with the specified icon paths applied.</returns>
    public PulldownButtonBuilder WithIcon(params string[] paths)
    {
        return RibbonExtensions.WithIcon(this, paths);
    }

    /// <summary>
    ///     Sets the tooltip text for the PulldownButton being built.
    /// </summary>
    /// <param name="tooltip">The tooltip text to be displayed for the PulldownButton.</param>
    /// <returns>The PulldownButtonBuilder instance with the specified tooltip applied.</returns>
    public PulldownButtonBuilder WithTooltip(string tooltip)
    {
        _tooltip = tooltip;
        return this;
    }
}
