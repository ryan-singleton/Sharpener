// The Sharpener project licenses this file to you under the MIT license.

using System.Text.Json;
using Sharpener.Json.Types.Interfaces;

namespace Sharpener.Json.Types;

/// <summary>
/// A deserializer for System.Text.Json.
/// </summary>
public class SystemFrom : IJsonDeserializer
{
    /// <inheritdoc/>
    public Func<string, Type, object?> Deserialize => (json, type) => JsonSerializer.Deserialize(json, type);
}
