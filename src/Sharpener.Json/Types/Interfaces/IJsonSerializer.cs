// The Sharpener project and Facefire license this file to you under the MIT license.

namespace Sharpener.Json.Types.Interfaces;

/// <summary>
/// The contract for a serializer for JSON.
/// </summary>
public interface IJsonSerializer
{
    /// <summary>
    /// The serialization logic.
    /// </summary>
    /// <value></value>
    Func<object, string> Serialize { get; }
}
