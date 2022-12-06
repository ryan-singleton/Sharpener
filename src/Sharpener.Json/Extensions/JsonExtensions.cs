// The Sharpener project and Facefire license this file to you under the MIT license.
using Sharpener.Json.Types;
using Sharpener.Json.Types.Interfaces;
namespace Sharpener.Json.Extensions;
/// <summary>
/// Extensions for serialization.
/// </summary>
public static class JsonExtensions
{
    private static IJsonWriter? s_cachedJsonWriter;
    private static IJsonReader? s_cachedJsonReader;
    /// <summary>
    /// Creates a JSON string according to the supplied type.
    /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <typeparam name="TWriter">The type of serializer to use.</typeparam>
    /// <returns></returns>
    public static string WriteJson<TWriter>(this object source) where TWriter : IJsonWriter, new()
    {
        if (s_cachedJsonWriter is not TWriter)
        {
            s_cachedJsonWriter = new TWriter();
        }
        return s_cachedJsonWriter.Write(source);
    }
    /// <summary>
    /// Creates a JSON string according to <see cref="SharpenerJsonSettings.GetDefaultWriter"/>.
    /// /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <returns></returns>
    public static string WriteJson(this object source)
    {
        var type = SharpenerJsonSettings.GetDefaultWriter();
        if (s_cachedJsonWriter?.GetType() != type)
        {
            s_cachedJsonWriter = Activator.CreateInstance(type) as IJsonWriter ?? throw new NullReferenceException("The default json writer was null");
        }
        return s_cachedJsonWriter!.Write(source);
    }
    /// <summary>
    /// Deserializes using JSON deserialization according to the supplied type.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    /// <typeparam name="TReader">The type of deserializer to use.</typeparam>
    public static TResult? ReadJsonAs<TResult, TReader>(this string json) where TResult : class where TReader : IJsonReader, new()
    {
        if (s_cachedJsonReader is not TReader)
        {
            s_cachedJsonReader = new TReader();
        }
        return s_cachedJsonReader.Read(json, typeof(TResult)) as TResult;
    }
    /// <summary>
    /// Deserializes using JSON deserialization according to <see cref="SharpenerJsonSettings.GetDefaultWriter"/>.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    public static TResult? ReadJsonAs<TResult>(this string json) where TResult : class
    {
        var type = SharpenerJsonSettings.GetDefaultReader();
        if (s_cachedJsonReader?.GetType() != type)
        {
            s_cachedJsonReader = Activator.CreateInstance(type) as IJsonReader ?? throw new NullReferenceException("The default json reader was null");
        }
        return s_cachedJsonReader.Read(json, typeof(TResult)) as TResult;
    }
}
