// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Revit.Ribbon;

/// <summary>
///     Represents an interface that defines an icon property for elements that support icons.
/// </summary>
public interface IHasIcon
{
    /// <summary>
    ///     Gets or sets the file path to the icon image associated with this element.
    /// </summary>
    /// <remarks>
    ///     This property specifies the location of an image file that can be used
    ///     as the graphical representation of a Revit ribbon button or an element.
    ///     The provided file path should point to a valid image file. If the path
    ///     is null, empty, or invalid, no icon will be displayed.
    /// </remarks>
    public string? IconPath { get; set; }
}
