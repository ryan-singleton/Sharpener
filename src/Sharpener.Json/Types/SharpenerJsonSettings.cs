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
    private static Func<object, string> _defaultWriter = default!;
    private static Func<string, Type, object?> _defaultReader = default!;
    /// <summary>
    /// Gets the default serializer.
    /// </summary>
    /// <returns></returns>
    public static Func<object, string> DefaultWriter => _defaultWriter;
    /// <summary>
    /// Gets the default deserializer.
    /// </summary>
    /// <returns></returns>
    public static Func<string, Type, object?> DefaultReader => _defaultReader;
    /// <summary>
    /// Sets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultWriter(Func<object, string> function) => _defaultWriter = function;
    /// <summary>
    /// <summary>
    /// Sets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultWriter<T>() where T : IJsonWriter, new() => _defaultWriter = new T().Write;
    /// <summary>
    /// Gets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultReader(Func<string, Type, object?> function) => _defaultReader = function;
    /// <summary>
    /// Gets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultReader<T>() where T : IJsonReader, new() => _defaultReader = new T().Read;
    /// <summary>
    /// Sets the JSON logic defaults back to System.Text.Json.
    /// </summary>
    public static void ResetDefaults()
    {
        SetDefaultWriter<JsonNetWriter>();
        SetDefaultReader<JsonNetReader>();
    }
}
