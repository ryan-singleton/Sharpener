namespace Sharpener.Types.Serialization.Interfaces;

/// <summary>
/// The contract for a deserializer for JSON.
/// </summary>
public interface IJsonDeserializer
{
    /// <summary>
    /// The deserialization logic.
    /// </summary>
    /// <value></value>
    Func<string, Type, object> Deserialize { get; }
}
