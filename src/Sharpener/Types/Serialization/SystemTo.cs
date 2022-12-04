using System.Text.Json;
using Sharpener.Types.Serialization.Interfaces;

namespace Sharpener.Types.Serialization;

/// <summary>
/// A serializer for System.Text.Json.
/// </summary>
public class SystemTo : IJsonSerializer
{
    private static readonly JsonSerializerOptions s_options = new() { WriteIndented = true };
    /// <inheritdoc/>
    public Func<object, string> Serialize => model => JsonSerializer.Serialize(model, s_options);
}
