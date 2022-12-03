using Sharpener.Preferences;

namespace Sharpener.Extensions;

/// <summary>
/// Extensions for serialization.
/// </summary>
public static class SerializationExtensions
{
    /// <summary>
    /// Creates JSON serialization according to registered functions in the application. If null, uses System.Text.Json defaults.
    /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <param name="functionName">The optional name of the registered function. Uses defaults if null.</param>
    /// <returns></returns>
    public static string ToJson(this object source, string? functionName = null)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            return SerializationSettings.DefaultToJson(source);
        }

        var function = SerializationSettings.GetNamedToJson(functionName);

        return function is null
            ? throw new ArgumentException("There is no to json function by the name {0}", functionName)
            : function(source);
    }

    /// <summary>
    /// Deserializes using JSON deserialization according to registered functions in the application. If null, uses System.Text.Json defaults.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    public static T? FromJson<T>(this string json, string? functionName = null) where T : class
    {
        if (string.IsNullOrEmpty(functionName))
        {
            return SerializationSettings.DefaultFromJson(json, typeof(T)) as T;
        }

        var function = SerializationSettings.GetNamedFromJson(functionName);

        return function is null
            ? throw new ArgumentException("There is no from json function by the name {0}", functionName)
            : function(json, typeof(T)) as T;
    }
}
