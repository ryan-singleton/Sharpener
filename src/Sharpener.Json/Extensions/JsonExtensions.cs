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
    /// Creates a JSON string according to <see cref="SharpenerJsonSettings.DefaultWriter"/>.
    /// /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <returns></returns>
    public static string WriteJson(this object source) => SharpenerJsonSettings.DefaultWriter(source);
    /// <summary>
    /// Deserializes using JSON deserialization according to the supplied type.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    /// <typeparam name="TReader">The type of deserializer to use.</typeparam>
    public static TResult? ReadJsonAs<TResult, TReader>(this string json) where TReader : IJsonReader, new()
    {
        if (s_cachedJsonReader is not TReader)
        {
            s_cachedJsonReader = new TReader();
        }
        return s_cachedJsonReader.Read(json, typeof(TResult)) is TResult result ? result : default(TResult);
    }
    /// <summary>
    /// Deserializes using JSON deserialization according to <see cref="SharpenerJsonSettings.DefaultWriter"/>.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    public static TResult? ReadJsonAs<TResult>(this string json) => SharpenerJsonSettings.DefaultReader(json, typeof(TResult)) is TResult result ? result : default(TResult);
}
