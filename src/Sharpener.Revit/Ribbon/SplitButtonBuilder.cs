// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a SplitButton in a Revit ribbon panel.
/// </summary>
public class SplitButtonBuilder : IRibbonElementBuilder
{
    private readonly List<IPushButtonBuilder> _buttonBuilders = [];
    private readonly string _name;

    /// <summary>
    ///     A class that provides methods for building and configuring a SplitButton element
    ///     to be added to a Revit ribbon panel.
    /// </summary>
    public SplitButtonBuilder(string name)
    {
        _name = name;
    }

    /// <summary>
    ///     Constructs a SplitButton within the specified RibbonPanel by adding all configured push buttons to it.
    /// </summary>
    /// <param name="panel">The RibbonPanel to which the SplitButton will be added.</param>
    public void Build(RibbonPanel panel)
    {
        var splitButtonData = new SplitButtonData(_name, _name);
        var splitButton = (SplitButton)panel.AddItem(splitButtonData);

        foreach (var buttonBuilder in _buttonBuilders)
        {
            splitButton.AddPushButton(buttonBuilder.ToPushButtonData());
        }
    }

    /// <summary>
    ///     Adds a new PushButton builder to the SplitButton configuration with the specified name, text,
    ///     and optional configuration action.
    /// </summary>
    /// <typeparam name="T">The type of the command to be executed when the button is clicked.</typeparam>
    /// <param name="name">The name of the PushButton.</param>
    /// <param name="text">The display text for the PushButton.</param>
    /// <param name="config">An optional action to configure the PushButtonBuilder.</param>
    /// <returns>The current instance of <see cref="SplitButtonBuilder" /> to allow for method chaining.</returns>
    public SplitButtonBuilder AddPushButton<T>(string name, string text, Action<PushButtonBuilder<T>>? config)
    {
        var pushButtonBuilder = new PushButtonBuilder<T>(name, text);
        config?.Invoke(pushButtonBuilder);
        _buttonBuilders.Add(pushButtonBuilder);
        return this;
    }
}
