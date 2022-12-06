// The Sharpener project and Facefire license this file to you under the MIT license.
using Sharpener.Json.Types.Interfaces;
namespace Sharpener.Json.Types;
/// <summary>
/// Global settings for all Sharpener JSON features.
/// </summary>
public struct SharpenerJsonSettings
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <returns></returns>
    static SharpenerJsonSettings() => ResetDefaults();
    private static Type _defaultWriter = default!;
    private static Type _defaultReader = default!;
    /// <summary>
    /// Gets the default serializer.
    /// </summary>
    /// <returns></returns>
    public static Type GetDefaultWriter() => _defaultWriter;
    /// <summary>
    /// Gets the default deserializer.
    /// </summary>
    /// <returns></returns>
    public static Type GetDefaultReader() => _defaultReader;
    /// <summary>
    /// Sets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultWriter<T>() where T : IJsonWriter, new() => _defaultWriter = typeof(T);
    /// <summary>
    /// Gets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultReader<T>() where T : IJsonReader, new() => _defaultReader = typeof(T);
    /// <summary>
    /// Sets the JSON logic defaults back to System.Text.Json.
    /// </summary>
    public static void ResetDefaults()
    {
        SetDefaultWriter<JsonNetWriter>();
        SetDefaultReader<JsonNetReader>();
    }
}
