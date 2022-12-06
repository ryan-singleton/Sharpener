// The Sharpener project and Facefire license this file to you under the MIT license.
using System.Text.Json;
using Sharpener.Json.Types.Interfaces;
namespace Sharpener.Json.Types;
/// <summary>
/// A deserializer for System.Text.Json.
/// </summary>
public class JsonNetReader : IJsonReader
{
    /// <inheritdoc/>
    public Func<string, Type, object?> Read => (json, type) => JsonSerializer.Deserialize(json, type);
}
