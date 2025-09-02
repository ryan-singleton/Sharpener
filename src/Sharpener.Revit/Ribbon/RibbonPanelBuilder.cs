// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a Revit ribbon panel.
/// </summary>
public class RibbonPanelBuilder
{
    private readonly List<IRibbonElementBuilder> _elementBuilders = [];
    private readonly string _panelName;

    /// <summary>
    ///     A class that provides methods for building and configuring a Revit ribbon panel.
    /// </summary>
    public RibbonPanelBuilder(string panelName)
    {
        _panelName = panelName;
    }

    /// <summary>
    ///     Adds a ComboBox to the Revit ribbon panel and allows for its configuration.
    /// </summary>
    /// <param name="name">The name of the ComboBox to add.</param>
    /// <param name="config">An optional configuration action to customize the ComboBox.</param>
    /// <returns>
    ///     An updated instance of <see cref="RibbonPanelBuilder" /> to enable method chaining for additional
    ///     configurations.
    /// </returns>
    public RibbonPanelBuilder AddComboBox(string name, Action<ComboBoxBuilder>? config)
    {
        var comboBoxBuilder = new ComboBoxBuilder(name);
        config?.Invoke(comboBoxBuilder);
        _elementBuilders.Add(comboBoxBuilder);
        return this;
    }

    /// <summary>
    ///     Adds a pulldown button to the Revit ribbon panel with the specified name, text, and configuration options.
    /// </summary>
    /// <param name="name">The internal name of the pulldown button.</param>
    /// <param name="text">The display text on the pulldown button.</param>
    /// <param name="config">An optional configuration action that defines the properties and behavior of the pulldown button.</param>
    /// <returns>The current instance of <see cref="RibbonPanelBuilder" /> to allow method chaining.</returns>
    public RibbonPanelBuilder AddPulldownButton(string name, string text, Action<PulldownButtonBuilder>? config)
    {
        var pulldownButtonBuilder = new PulldownButtonBuilder(name, text);
        config?.Invoke(pulldownButtonBuilder);
        _elementBuilders.Add(pulldownButtonBuilder);
        return this;
    }

    /// <summary>
    ///     Adds a new push button to the ribbon panel with the specified name, text, and configuration.
    /// </summary>
    /// <typeparam name="T">The type of the command to be executed when the button is clicked.</typeparam>
    /// <param name="name">The unique name of the push button.</param>
    /// <param name="text">The display text of the push button.</param>
    /// <param name="config">An optional configuration action for customizing the push button.</param>
    /// <returns>Returns the current instance of <see cref="RibbonPanelBuilder" /> for method chaining.</returns>
    public RibbonPanelBuilder AddPushButton<T>(string name, string text, Action<PushButtonBuilder<T>>? config)
    {
        var pushButtonBuilder = new PushButtonBuilder<T>(name, text);
        config?.Invoke(pushButtonBuilder);
        _elementBuilders.Add(pushButtonBuilder);
        return this;
    }

    /// <summary>
    ///     Adds a push button to the ribbon panel using the specified builder.
    /// </summary>
    /// <param name="pushButtonBuilder">The builder for the push button to be added.</param>
    /// <returns>Returns the current instance of <see cref="RibbonPanelBuilder" /> for method chaining.</returns>
    public RibbonPanelBuilder AddPushButton<T>(PushButtonBuilder<T> pushButtonBuilder)
    {
        _elementBuilders.Add(pushButtonBuilder);
        return this;
    }

    /// <summary>
    ///     Adds a separator to the ribbon panel.
    /// </summary>
    /// <returns>Returns the current instance of <see cref="RibbonPanelBuilder" /> for method chaining.</returns>
    public RibbonPanelBuilder AddSeparator()
    {
        _elementBuilders.Add(new SeparatorBuilder());
        return this;
    }

    /// <summary>
    ///     Adds a split button to the ribbon panel with the specified name and configuration.
    /// </summary>
    /// <param name="name">The unique name of the split button.</param>
    /// <param name="config">An optional configuration action for customizing the split button.</param>
    /// <returns>Returns the current instance of <see cref="RibbonPanelBuilder" /> for method chaining.</returns>
    public RibbonPanelBuilder AddSplitButton(string name, Action<SplitButtonBuilder>? config)
    {
        var splitButtonBuilder = new SplitButtonBuilder(name);
        config?.Invoke(splitButtonBuilder);
        _elementBuilders.Add(splitButtonBuilder);
        return this;
    }

    /// <summary>
    ///     Adds a text box to the ribbon panel with the specified name and configuration.
    /// </summary>
    /// <param name="name">The unique name of the text box.</param>
    /// <param name="prompt">The text to be displayed in the text box.</param>
    /// <returns>Returns the current instance of <see cref="RibbonPanelBuilder" /> for method chaining.</returns>
    public RibbonPanelBuilder AddTextBox(string name, string prompt = "")
    {
        _elementBuilders.Add(new TextBoxBuilder(name, prompt));
        return this;
    }

    /// <summary>
    ///     Builds the ribbon panel with the specified name and configuration.
    /// </summary>
    /// <param name="app">The Revit application.</param>
    /// <param name="name">The name of the ribbon panel.</param>
    public void Build(UIControlledApplication app, string name)
    {
        var panel = app.CreateRibbonPanel(name, _panelName);

        foreach (var elementBuilder in _elementBuilders)
        {
            elementBuilder.Build(panel);
        }
    }
}
