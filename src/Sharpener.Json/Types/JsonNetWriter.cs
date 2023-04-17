// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Json.Types;

using System.Text.Json;
using Interfaces;

/// <summary>
///     A serializer for System.Text.Json.
/// </summary>
public class JsonNetWriter : IJsonWriter
{
    private static readonly JsonSerializerOptions SOptions = new() { WriteIndented = true };

    /// <inheritdoc />
    public Func<object, string> Write => model => JsonSerializer.Serialize(model, SOptions);
}
