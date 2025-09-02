// The Sharpener project licenses this file to you under the MIT license.

using System.Windows;

namespace Sharpener.Revit.Ui;

/// <summary>
///     A reference for defining the options of Revit theme event synchronization.
/// </summary>
/// <typeparam name="T">The dependency object affected by the theme changes.</typeparam>
public class SyncRevitThemeOptions<T> where T : DependencyObject
{
    /// <summary>
    ///     The action to take when the Revit editor theme changes.
    /// </summary>
    public Action<T>? OnCanvasThemeChanged { get; set; }

    /// <summary>
    ///     The action to take on Revit startup.
    /// </summary>
    public Action<T>? OnStartup { get; set; }

    /// <summary>
    ///     The action to take when the Revit UI theme changes.
    /// </summary>
    public Action<T>? OnUiThemeChanged { get; set; }
}
