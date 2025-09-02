// The Sharpener project licenses this file to you under the MIT license.

using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Sharpener.Revit.Ribbon;

namespace Sharpener.Revit.Extensions;

/// <summary>
///     Provides extension methods for working with Revit ribbon elements.
/// </summary>
public static class RibbonExtensions
{
    /// <summary>
    ///     Gets the icon image for the given element.
    /// </summary>
    /// <param name="self">The element to get the icon for.</param>
    /// <returns>The icon image, or null if the element does not have an icon.</returns>
    public static BitmapImage? GetIcon<T>(this T self) where T : IHasIcon
    {
        if (string.IsNullOrEmpty(self.IconPath) || !File.Exists(self.IconPath))
        {
            return null;
        }

        return new BitmapImage(new Uri(self.IconPath!, UriKind.Absolute));
    }

    /// <summary>
    ///     Sets the icon for the given element.
    /// </summary>
    /// <param name="self">The element to set the icon for.</param>
    /// <param name="paths">The paths to the icon files.</param>
    /// <returns>The element with the icon set.</returns>
    public static T WithIcon<T>(this T self, params string[] paths) where T : IHasIcon
    {
        var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var prependedPaths = paths.Prepend(assemblyDir).ToArray();
        if (string.IsNullOrWhiteSpace(assemblyDir))
        {
            throw new ArgumentException("The executing assembly's directory was null or empty.");
        }

        self.IconPath = Path.Combine(prependedPaths);
        return self;
    }
}
