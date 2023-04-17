// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Json.Types.Interfaces;

/// <summary>
///     The contract for a deserializer for JSON.
/// </summary>
public interface IJsonReader
{
    /// <summary>
    ///     The deserialization logic.
    /// </summary>
    /// <value></value>
    Func<string, Type, object?> Read { get; }
}
