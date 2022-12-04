using Sharpener.Types.Serialization;
using Sharpener.Types.Serialization.Interfaces;

namespace Sharpener.Extensions;

/// <summary>
/// Extensions for serialization.
/// </summary>
public static class SerializationExtensions
{
    private static IJsonSerializer? s_jsonSerializer;

    private static IJsonDeserializer? s_jsonDeserializer;


    /// <summary>
    /// Creates JSON serialization according to the supplied type.
    /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <typeparam name="TSerializer">The type of serializer to use.</typeparam>
    /// <returns></returns>
    public static string ToJson<TSerializer>(this object source) where TSerializer : IJsonSerializer, new()
    {
        if (s_jsonSerializer is not TSerializer)
        {
            s_jsonSerializer = new TSerializer();
        }

        return s_jsonSerializer.Serialize(source);
    }

    /// <summary>
    /// Creates JSON serialization according to <see cref="SharpenerJsonSettings.GetDefaultSerializer"/>.
    /// /// </summary>
    /// <param name="source">The reference to serialize.</param>
    /// <returns></returns>
    public static string ToJson(this object source)
    {
        Type type = SharpenerJsonSettings.GetDefaultSerializer();
        if (s_jsonSerializer?.GetType() != type)
        {
            s_jsonSerializer = Activator.CreateInstance(type) as IJsonSerializer ?? throw new NullReferenceException("The default json serializer was null");
        }

        return s_jsonSerializer!.Serialize(source);
    }

    /// <summary>
    /// Deserializes using JSON deserialization according to the supplied type.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    /// <typeparam name="TDeserializer">The type of deserializer to use.</typeparam>
    public static TResult? FromJson<TResult, TDeserializer>(this string json) where TResult : class where TDeserializer : IJsonDeserializer, new()
    {
        if (s_jsonDeserializer is not TDeserializer)
        {
            s_jsonDeserializer = new TDeserializer();
        }

        return s_jsonDeserializer.Deserialize(json, typeof(TResult)) as TResult;
    }

    /// <summary>
    /// Deserializes using JSON deserialization according to <see cref="SharpenerJsonSettings.GetDefaultSerializer"/>.
    /// </summary>
    /// <typeparam name="TResult">The type to deserialize to.</typeparam>
    public static TResult? FromJson<TResult>(this string json) where TResult : class
    {
        Type type = SharpenerJsonSettings.GetDefaultDeserializer();
        if (s_jsonDeserializer?.GetType() != type)
        {
            s_jsonDeserializer = Activator.CreateInstance(type) as IJsonDeserializer ?? throw new NullReferenceException("The default json deserializer was null");
        }

        return s_jsonDeserializer.Deserialize(json, typeof(TResult)) as TResult;
    }
}
