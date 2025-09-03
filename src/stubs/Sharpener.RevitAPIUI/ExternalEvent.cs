// The Sharpener project licenses this file to you under the MIT license.

namespace Autodesk.Revit.UI;

public class ExternalEvent : IDisposable
{
    public void Dispose()
    {
    }

    public static ExternalEvent Create(object handler)
    {
        return new ExternalEvent();
    }

    public void Raise()
    {
    }
}
