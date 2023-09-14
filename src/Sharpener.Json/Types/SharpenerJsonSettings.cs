// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Json.Types.Interfaces;

namespace Sharpener.Json.Types;

/// <summary>
///     Global settings for all Sharpener JSON features.
/// </summary>
public struct SharpenerJsonSettings
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <returns></returns>
    static SharpenerJsonSettings()
    {
        ResetDefaults();
    }

    /// <summary>
    ///     Gets the default serializer.
    /// </summary>
    /// <returns></returns>
    public static Func<object, string> DefaultWriter { get; private set; } = default!;

    /// <summary>
    ///     Gets the default deserializer.
    /// </summary>
    /// <returns></returns>
    public static Func<string, Type, object?> DefaultReader { get; private set; } = default!;

    /// <summary>
    ///     Sets the default serializer.
    /// </summary>
    /// <param name="function"></param>
    public static void SetDefaultWriter(Func<object, string> function)
    {
        DefaultWriter = function;
    }

    /// <summary>
    ///     Sets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultWriter<T>() where T : IJsonWriter, new()
    {
        DefaultWriter = new T().Write;
    }

    /// <summary>
    ///     Gets the default serializer.
    /// </summary>
    /// <param name="function"></param>
    public static void SetDefaultReader(Func<string, Type, object?> function)
    {
        DefaultReader = function;
    }

    /// <summary>
    ///     Gets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultReader<T>() where T : IJsonReader, new()
    {
        DefaultReader = new T().Read;
    }

    /// <summary>
    ///     Sets the JSON logic defaults back to System.Text.Json.
    /// </summary>
    public static void ResetDefaults()
    {
        SetDefaultWriter<JsonNetWriter>();
        SetDefaultReader<JsonNetReader>();
    }
}
