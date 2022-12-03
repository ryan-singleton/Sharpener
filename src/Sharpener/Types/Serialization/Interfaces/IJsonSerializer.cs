namespace Sharpener.Types.Serialization.Interfaces;

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
