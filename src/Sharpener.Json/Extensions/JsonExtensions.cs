// The Sharpener project licenses this file to you under the MIT license.

using Sharpener.Json.Types;
using Sharpener.Json.Types.Interfaces;

namespace Sharpener.Json.Extensions;

/// <summary>
///     Extensions for serialization.
/// </summary>
public static class JsonExtensions
{
    private static IJsonWriter? _sCachedJsonWriter;
    private static IJsonReader? _sCachedJsonReader;

    /// <summary>
    ///     Deserializes using JSON deserialization according to the supplied type.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    /// <typeparam name="TReader">The type of deserializer to use.</typeparam>
    public static TResult? ReadJsonAs<TResult, TReader>(this string json) where TReader : IJsonReader, new()
    {
        if (_sCachedJsonReader is not TReader)
        {
            _sCachedJsonReader = new TReader();
        }

        return _sCachedJsonReader.Read(json, typeof(TResult)) is TResult result ? result : default;
    }

    /// <summary>
    ///     Deserializes using JSON deserialization according to <see cref="SharpenerJsonSettings.DefaultWriter" />.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    public static TResult? ReadJsonAs<TResult>(this string json)
    {
        return SharpenerJsonSettings.DefaultReader(json, typeof(TResult)) is TResult result ? result : default;
    }

    /// <summary>
    ///     Creates a JSON string according to the supplied type.
    /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <typeparam name="TWriter">The type of serializer to use.</typeparam>
    /// <returns></returns>
    public static string WriteJson<TWriter>(this object source) where TWriter : IJsonWriter, new()
    {
        if (_sCachedJsonWriter is not TWriter)
        {
            _sCachedJsonWriter = new TWriter();
        }

        return _sCachedJsonWriter.Write(source);
    }

    /// <summary>
    ///     Creates a JSON string according to <see cref="SharpenerJsonSettings.DefaultWriter" />.
    ///     ///
    /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <returns>  The JSON formatted string.</returns>
    public static string WriteJson(this object source)
    {
        return SharpenerJsonSettings.DefaultWriter(source);
    }
}
