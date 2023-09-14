// The Sharpener project licenses this file to you under the MIT license.

using System.Text.Json;
using Sharpener.Json.Types.Interfaces;

namespace Sharpener.Json.Types;

/// <summary>
///     A serializer for System.Text.Json.
/// </summary>
public class JsonNetWriter : IJsonWriter
{
    private static readonly JsonSerializerOptions SOptions = new() { WriteIndented = true };

    /// <inheritdoc />
    public Func<object, string> Write => model => JsonSerializer.Serialize(model, SOptions);
}
