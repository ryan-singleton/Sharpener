// The Sharpener project licenses this file to you under the MIT license.

using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.UI;
using Sharpener.Revit.Extensions;

namespace Sharpener.Revit.Controls;

/// <summary>
///     A convenient subclass that helps to stand up a dockable pane. Dockable Revit pages can be created with this easily.
/// </summary>
public class RevitPane : Page, IDockablePaneProvider
{
    /// <summary>
    ///     Instantiates a new Revit dockable pane.
    /// </summary>
    /// <param name="uiApplication">The associated <see cref="UIControlledApplication" /> for this pane.</param>
    /// <param name="paneName">The name of the pane, which can be searched for later, or can feed its own labels and controls.</param>
    protected RevitPane(UIControlledApplication uiApplication, string paneName)
    {
        PaneName = paneName;
        DockablePaneId = new DockablePaneId(Guid.NewGuid());
        uiApplication.SyncRevitTheme(this, options =>
        {
            options.OnStartup = ApplySynchronizedTheme;
            options.OnUiThemeChanged = ApplySynchronizedTheme;
        });
    }

    /// <summary>
    ///     The unique identifier of the dockable pane.
    /// </summary>
    public DockablePaneId DockablePaneId { get; }

    /// <summary>
    ///     The name of the pane, which can be searched for later, or can feed its own labels and controls.
    /// </summary>
    public string PaneName { get; }

    /// <inheritdoc />
    public virtual void SetupDockablePane(DockablePaneProviderData data)
    {
        data.FrameworkElement = this;
        data.InitialState = new DockablePaneState();
    }

    private static void ApplySynchronizedTheme(DependencyObject dependencyObject)
    {
        //todo: we need to build UI libraries with the two themes to actually use this, but keep it around for a minute
        //dependencyObject.ApplyTheme(UIThemeManager.CurrentTheme == UITheme.Dark ? Theme.Dark : Theme.Light);
    }
}
