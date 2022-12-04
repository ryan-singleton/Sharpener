using System.Text.Json;
using Sharpener.Types.Serialization.Interfaces;

namespace Sharpener.Types.Serialization;

/// <summary>
/// A deserializer for System.Text.Json.
/// </summary>
public class SystemFrom : IJsonDeserializer
{
    /// <inheritdoc/>
    public Func<string, Type, object?> Deserialize => (json, type) => JsonSerializer.Deserialize(json, type);
}
