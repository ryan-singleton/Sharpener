// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents a builder for creating a Revit ribbon tab.
/// </summary>
public class RibbonTabBuilder
{
    private readonly List<RibbonPanelBuilder> _panels = [];
    private readonly string _tabName;

    private RibbonTabBuilder(string tabName)
    {
        _tabName = tabName;
    }

    /// <summary>
    ///     Adds a Revit ribbon panel to the tab being built.
    /// </summary>
    /// <param name="name">The name of the ribbon panel to be added.</param>
    /// <param name="config">An optional action to configure the ribbon panel being added.</param>
    /// <returns>The <see cref="RibbonTabBuilder" /> instance.</returns>
    public RibbonTabBuilder AddPanel(string name, Action<RibbonPanelBuilder>? config)
    {
        var panelBuilder = new RibbonPanelBuilder(name);
        config?.Invoke(panelBuilder);
        _panels.Add(panelBuilder);
        return this;
    }

    /// <summary>
    ///     Creates the Revit ribbon tab and builds all associated panels within it.
    /// </summary>
    /// <param name="app">The Revit application used to create the ribbon tab and panels.</param>
    public void Build(UIControlledApplication app)
    {
        app.CreateRibbonTab(_tabName);

        foreach (var panelBuilder in _panels)
        {
            panelBuilder.Build(app, _tabName);
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RibbonTabBuilder" /> class with a specified tab name.
    /// </summary>
    /// <param name="tabName">The name of the ribbon tab to be created.</param>
    /// <returns>A new instance of the <see cref="RibbonTabBuilder" /> class.</returns>
    public static RibbonTabBuilder Create(string tabName)
    {
        return new RibbonTabBuilder(tabName);
    }
}
