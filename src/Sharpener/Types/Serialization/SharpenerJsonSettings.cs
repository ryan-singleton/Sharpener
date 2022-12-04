using Sharpener.Types.Serialization.Interfaces;

namespace Sharpener.Types.Serialization;

/// <summary>
/// Global settings for all Sharpener JSON features.
/// </summary>
public static class SharpenerJsonSettings
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <returns></returns>
    static SharpenerJsonSettings() => ResetDefaults();
    private static Type _defaultSerializer = default!;
    private static Type _defaultDeserializer = default!;

    /// <summary>
    /// Gets the default serializer.
    /// </summary>
    /// <returns></returns>
    public static Type GetDefaultSerializer() => _defaultSerializer;

    /// <summary>
    /// Gets the default deserializer.
    /// </summary>
    /// <returns></returns>
    public static Type GetDefaultDeserializer() => _defaultDeserializer;

    /// <summary>
    /// Sets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultSerializer<T>() where T : IJsonSerializer, new() => _defaultSerializer = typeof(T);

    /// <summary>
    /// Gets the default serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SetDefaultDeserializer<T>() where T : IJsonDeserializer, new() => _defaultDeserializer = typeof(T);

    /// <summary>
    /// Sets the JSON logic defaults back to System.Text.Json.
    /// </summary>
    public static void ResetDefaults()
    {
        SetDefaultSerializer<SystemTo>();
        SetDefaultDeserializer<SystemFrom>();
    }
}
