# Named Resources

To understand named resources, you should know the dilemma. And that comes from a niche application downstream.

## The challenge

I work on solutions that utilize dependencies that are not always built from C#. Often C++. And so you don't get the
common advantages of the framework for resilient code, like composition with interfaces, or generic types. Whether
it's possible in C++ is not the point, it's that the immutable dependencies I work with *don't*.

There are reference types that can have a wide variety of members. For example, a geometric object with dimensions.
Depending on the predetermined shape type, it could have a diameter, width, height, depth, length, angle, and so on.
And it can have any combination of those.

The dependency allows me to write code like this:

```csharp
extension(GeometryItem item)
{
    public double? GetDimension(string dimensionName)
    {
        // Often need to wrap in try catch because the interop can randomly cause data access failures that you
        // cannot predict
        try
        {
            var dimension = item.Dimensions.FirstOrDefault(dimension => dimension.Name.Equals(dimensionName, StringComparison.OrdinalIgnoreCase));
            if (dimension?.IsValidObject == true && dimension.StorageType == StorageType.Double || dimension.Value is double doubleValue) return doubleValue;
        }
        catch
        {
            // Track the failure, maybe.
        }
        return null;
    }
}
```

However, even after writing tooling like this, a larger application development efforts starts to really challenge you
to avoid creating tech debt. Riddled throughout your application you'll start to see a whole lot of these:

```csharp
var topExtension = item.GetDimension("Top Extension");
if(topExtension is null) return;

// or

var flatTop = item.GetOptionValue("Flat Top");
```

And this just gets repeated all over the place. Some of the option names are also quirky, like "Btm Width" instead of
explicit "Bottom", or the regional dialect comes into play because someone from the UK built the dependency so it's a
"Mitred" option for a corner instead of "Mitered".

You just want to define these string values once and be done with it! Ok, so what's wrong with this?

```csharp
public static class Dimensions
{
    // document everything too dude
    public const string TopExtension = "Top Extension"
}
```

So I can write

```csharp
var topExtension = item.GetDimension(Dimensions.TopExtension)
```

This is starting to look better, but it's more text to achieve a static value. Right? I mean, this is solid enough that,
given no other options, I'd say this works. But I'm making a package here and that package aims to just improve life
by that little bit.

So what does this package do?

## The Solution

Start off by creating an enum with the values that you want to be able to lookup by.

```csharp
 /// <summary>
 ///     The common dimensions found on items.
 /// </summary>
public enum DimensionType
{
    /// <summary>
    ///     The width dimension of an item.
    /// </summary>
    [Description(nameof(Width))]
    Width,
    /// <summary>
    ///     The top extension dimension of an item.
    /// </summary>
    [Description("Top Extension")]
    TopExtension
}
```

Then create an interface that inherits `INamedResource`.

```csharp
public interface IDimensionName
{
}
```

Now decorate `DimensionType` to indicate that you want it to generate code for you to use, based on the enum.

```csharp
 /// <summary>
 ///     The common dimensions found on items.
 /// </summary>
 [NamedResource(typeof(IDimensionName))]
public enum DimensionType
```

Now you do not need to create that static class with the strings in it.

`Dimensions.TopExtension` was created for you.

But something else was created for you too, and that is a static type. It is meant for making some more advanced
signatures.

```csharp
var topExtension = new TopExtension();

// This comes out as "Top Extension"
topExtension.Name;
```

Well, what am I gonna do with that?

```csharp
extension(GeometryItem item)
{
    public double? Dim<TDim>() where TDim : IDimensionName, new() => item.GetDimension(new TDim().Name) ?? double.NaN;
    public double? GetDimension(string dimensionName)
    {
        ///
    }
}
```

And so now, my typing can be a little more succinct and strong.

```csharp
var widthIsLarge = item.Dim<Width>() > 12;

// or

var isFlatTop = item.Option<OffsetDepth>() == Choices.FlatTop;
```

And what's nice is that the consistency between these two values

```csharp
Dimensions.FlatTop
// And
new FlatTop().Name
```

Will be predictable and reliable, because they are sourced by the enum that you maintain, the descriptions you put on
them, and the summaries you provide in the xml documentation.

## Overrides

You can override the defaults as well.

The constant strings are written to `Dimensions` because it removes "Name" from `IDimensionName` end, then if there are
two upper case letters in its typename, it removes the first, and lastly, it adds an "s" to the end.

```csharp
[NamedResource(typeof(IDimensionName))]

// results in

Dims.Width == "Width"
```

The other optional attribute parameters govern the namespace that the generated values are placed in, which defaults to
the same namespace as `IDimensionName`, and you can override the intellisense documentation for the `Dimensions` class
output.
