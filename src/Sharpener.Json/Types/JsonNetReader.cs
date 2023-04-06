// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Json.Types;

using System.Text.Json;
using Interfaces;

/// <summary>
///     A deserializer for System.Text.Json.
/// </summary>
public class JsonNetReader : IJsonReader
{
    /// <inheritdoc />
    public Func<string, Type, object?> Read => (json, type) => JsonSerializer.Deserialize(json, type);
}
