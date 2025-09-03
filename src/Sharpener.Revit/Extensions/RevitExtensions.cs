// The Sharpener project licenses this file to you under the MIT license.

using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Sharpener.Extensions;
using Sharpener.Revit.Selection;
using Sharpener.Revit.Ui;

namespace Sharpener.Revit.Extensions;

/// <summary>
///     Extensions pertaining to the Revit UI.
/// </summary>
public static class RevitExtensions
{
    /// <summary>
    ///     Retrieves the selected elements of type <typeparamref name="T" /> from the Revit document.
    ///     If no elements are currently selected, prompts the user to pick elements of type <typeparamref name="T" /> from the
    ///     document.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of elements to select or retrieve, which must derive from
    ///     <see cref="Autodesk.Revit.DB.Element" />.
    /// </typeparam>
    /// <param name="uiDocument">The Revit <see cref="Autodesk.Revit.UI.UIDocument" /> instance to retrieve elements from.</param>
    /// <param name="prompt">
    ///     An optional prompt message displayed when the user is asked to pick elements. Defaults to "Select
    ///     elements" if not provided.
    /// </param>
    /// <returns>An <see cref="IEnumerable{T}" /> containing the selected or picked elements of type <typeparamref name="T" />.</returns>
    public static IEnumerable<T> GetSelectedOrPickedItems<T>(this UIDocument uiDocument, string? prompt = null)
        where T : Element
    {
        var ids = uiDocument.Selection.GetElementIds();
        var elementsAsT = ids.Select(id => uiDocument.Document.GetElement(id)).OfType<T>().AsArray();
        if (!elementsAsT.IsNullOrEmpty())
        {
            return elementsAsT;
        }

        var selectedRef = uiDocument.Selection.PickObjects(ObjectType.Element,
            new ElementTypeSelectionFilter<T>(),
            string.IsNullOrWhiteSpace(prompt) ? "Select elements" : prompt);

        return selectedRef.Select(r => uiDocument.Document.GetElement(r)).OfType<T>().AsArray();
    }

    /// <summary>
    ///     Synchronizes the given <see cref="DependencyObject" />, often a <see cref="Window" /> or <see cref="Page" />, with
    ///     the current theme in the current Revit UI application. This also subscribes to change events in Revit so that the
    ///     synchronicity stays up to date for that Revit session.
    /// </summary>
    /// <param name="application">The current Revit UI application.</param>
    /// <param name="dependencyObject">The UI element whose theme should synchronize with Revit.</param>
    /// <param name="optAction">The optional action to take on the synchronization settings before applying them.</param>
    /// <typeparam name="T">The type of the dependency object. Often a <see cref="Window" /> or <see cref="Page" />.</typeparam>
    public static void SyncRevitTheme<T>(this UIControlledApplication application, T dependencyObject,
        Action<SyncRevitThemeOptions<T>> optAction) where T : DependencyObject
    {
        var options = new SyncRevitThemeOptions<T>();
        optAction(options);

        options.OnStartup?.Invoke(dependencyObject);
        application.ThemeChanged += (sender, args) =>
        {
            var currentFunc = args.ThemeChangedType == ThemeType.UITheme
                ? options.OnUiThemeChanged
                : options.OnCanvasThemeChanged;
            currentFunc?.Invoke(dependencyObject);
        };
    }
}
