// The Sharpener project and Facefire license this file to you under the MIT license.

namespace Sharpener.Json.Types.Interfaces;

/// <summary>
/// The contract for a deserializer for JSON.
/// </summary>
public interface IJsonDeserializer
{
    /// <summary>
    /// The deserialization logic.
    /// </summary>
    /// <value></value>
    Func<string, Type, object?> Deserialize { get; }
}
