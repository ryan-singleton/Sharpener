// The Sharpener project licenses this file to you under the MIT license.

using System.Text.Json;
using Sharpener.Json.Types.Interfaces;

namespace Sharpener.Json.Types;

/// <summary>
/// A serializer for System.Text.Json.
/// </summary>
public class SystemTo : IJsonSerializer
{
    private static readonly JsonSerializerOptions s_options = new() { WriteIndented = true };
    /// <inheritdoc/>
    public Func<object, string> Serialize => model => JsonSerializer.Serialize(model, s_options);
}
