// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Defines the contract for building push button elements within a Revit ribbon interface.
/// </summary>
public interface IPushButtonBuilder
{
    /// <summary>
    ///     Creates and returns a new PushButtonData object initialized with the required parameters
    ///     such as name, text, assembly path, and class name. Optionally, it applies the tooltip and icon
    ///     if they are provided.
    /// </summary>
    /// <returns>
    ///     A PushButtonData object that can be added to a Revit ribbon panel.
    /// </returns>
    PushButtonData ToPushButtonData();
}
