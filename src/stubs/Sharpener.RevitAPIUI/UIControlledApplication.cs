// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.UI;

public class UIControlledApplication
{
    public RibbonPanel CreateRibbonPanel(string name, string panelName)
    {
        return new RibbonPanel();
    }

    public void CreateRibbonTab(string tabName)
    {

    }

    public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs args);

    public event ThemeChangedEventHandler ThemeChanged;

}
