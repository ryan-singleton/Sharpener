<img src="images/icon-white-stroke-40px.png"
     align="right"
     style="height: 40px;" />
# Sharpener.Json
A small scoped package that extends the base `Sharpener` package as well as `System.Text.Json` for serialization and deserializationn.
## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Extended Features](#extended-features)
- [Support](#support)
- [Contributing](#contributing)
## Features
The `Sharpener.Json` package focuses on features for the basic runtime layer's json tooling.
- An `object` `WriteJson` extension.
- A `string` `ReadJsonAs<T>` extension.
- Configuration options for default behavior of these extensions.
- Type parameters for outlier serialization handling scenarios.
## Installation
Navigate to the directory of your .csproj file and then run this command
`dotnet add package Sharpener.Json`
or
`PM> Install-Package Sharpener.Json`
## Usage
### WriteJson
To serialize something, you can now just write this.
```cs
var json = item.WriteJson();
```
Want it to stop writing indented? Create a writer.
```cs
/// <summary>
/// A serializer for System.Text.Json that does not indent.
/// </summary>
public class JsonRawWriter : IJsonWriter
{
    private static JsonSerializerOptions s_options = new JsonSerializerOptions { WriteIndented = false };
    /// <inheritdoc/>
    public Func<object, string> Write => model => JsonSerializer.Serialize(model, s_options);
}
```
And now you can either explicitly use it, or register it as the default.
Register as default:
```cs
SharpenerJsonSettings.SetDefaultWriter<JsonRawWriter>();
var json = item.WriteJson();
```
or
```cs
SharpenerJsonSettings.SetDefaultWriter((model =>
    JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = false })))
var json = item.WriteJson();
```
Explicit call, which is better for scenarios where you'd prefer not to change the default and you're accomodating an outlier scenario.
```cs
var json = item.WriteJson<JsonRawWriter>();
```
### ReadJsonAs`<TResult>`
Let's deserialize something. I've got some text that can deserilialize to an `Item`.
```cs
var item = json.ReadJsonAs<Item>();
```
And similar to the `WriteJson` options above, you can add a custom deserializer of your own for `ReadJsonAs` as well.
