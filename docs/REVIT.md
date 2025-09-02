# Sharpener Revit API Tools

The tools in the `Sharpener.Revit` package are purpose built by a regular user of the `Revit.API` and `Revit.API.UI`
assemblies. It identifies opportunities to make these libraries more enjoyable to use.

> **Note:** Usage of this NuGet will require the referencing of the RevitAPI.dll and RevitAPIUI.dll. It does not come as
> a source for those binaries.

> **Further Note:** This package currently only supports Revit 2026 but analysis has begun on how to abstract so that
> usage of this package can be more universal, but there are some abstraction concepts to consider.

## Features

### Ribbon Builders

Customizing the Revit ribbon can be tedious, but using the `Sharpener.Revit.Ribbon` tooling, your ribbon syntax can
be more like this:

```csharp
private static void BuildRibbon(UIControlledApplication uiApplication)
{
    RibbonTabBuilder
        .Create("Your Revit AddIn")
        .AddPanel("Main", panel => panel
            .AddPushButton<ToggleMain>("Dashboard", "Dashboard", button => button
                .WithIcon("Resources", "dashboard.png"))
        )
        .AddPanel("Content", panel => panel
            .AddPushButton<ToggleContentManager>("Content Manager", "Content Manager", button => button
                .WithIcon("Resources", "content.png"))
        )
        .AddPanel("Mechanical", panel => panel
            .AddPushButton<CreateCutLengths>("Cut Lengths", "Cut Lengths", button => button
                .WithIcon("Resources", "content.png"))
        )
        .AddPanel("Settings", panel => panel
            .AddPushButton<OpenSettingsWindow>("Settings", "Settings", button => button
                .WithIcon("Resources", "settings.png")))
        .Build(uiApplication);
}
```

---

### Revit Theme

You can also sync your UI to the Revit theme

```csharp
// in your constructor or something
uiApplication.SyncRevitTheme(this, options =>
{
    options.OnStartup = ApplySynchronizedTheme;
    options.OnUiThemeChanged = ApplySynchronizedTheme;
});

// what you subscribe to
private static void ApplySynchronizedTheme(DependencyObject dependencyObject)
{
    // everything inside ApplyTheme is not real (yet?), it's just a sample of how you'd do it.
    dependencyObject.ApplyTheme(UIThemeManager.CurrentTheme == UITheme.Dark ? Theme.Dark : Theme.Light);
}
```

---

### Revit Panes

These controls are derived from `System.Windows.Controls.Page` and `Autodesk.Revit.UI.IDockablePaneProvider` and they
take care of a lot of the boilerplate for you so that you do not need to register it or anything like that. Just create
a class derived from this, then distribute it through your instance as needed (but I recommend dependency injection for
lifetime scope simplification).

```xaml
<panes:RevitPane x:Class="YourNameSpace.Controls.ContentPane"
                  # standard xaml declarations
                  xmlns:panes="clr-namespace:Sharpener.Revit.Controls.Panes;assembly=Sharpener.Revit">
    # your content
</panes:RevitPane>
```

---

### Element Type Selection Filter

A generic selection filter that simplifies filtering by element type.
Useful for avoiding redundant custom filters.

```csharp
var selectedRef = uiDoc.Selection.PickObject(ObjectType.Element, new ElementTypeSelectionFilter<FamilyInstance>(), "Select a family instance to register");
```

---

### Tryable External Command

Simplifies error handling in external commands by wrapping execution in a try-catch pattern.
Focus on the logic, not the boilerplate.

```csharp
/// <summary>
///     Upserts a family to a hypothetical family manager service.
/// </summary>
[TryableCommand]
[Transaction(TransactionMode.Manual)]
public class RegisterFamily : TryableExternalCommand
{
    protected override UiResult TryExecute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiApp = commandData.Application;

        // This might fail, but the base class handles exceptions
        var upsertResult = familyManagerService.PickAndUpsertContent(uiApp);
        return familyManagerService.HandleResult(uiApp, registrationResult, out message);
    }
}
```

---

### External Event Manager

Simplifies the management of external events.
Once registered (typically via dependency injection), you can raise events by handler type—no need for enums or manual
mapping.

```csharp
// Register the event
_externalEventManager.RegisterEvent(new FooEventHandler());

// Raise the event elsewhere
_externalEventManager.RaiseEvent<FooEventHandler>();

// Or bind to a WPF ICommand
ICommand FooCommand = new RelayCommand(() =>
    _externalEventManager.RaiseEvent<FooEventHandler>());
```

> This approach reduces boilerplate and enables clean `ICommand` bindings in WPF view models.
