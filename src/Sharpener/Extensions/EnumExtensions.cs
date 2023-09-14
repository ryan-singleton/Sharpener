// The Sharpener project licenses this file to you under the MIT license.

using System.ComponentModel;

namespace Sharpener.Extensions;

/// <summary>
///     Extensions for <see cref="Enum" /> references.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Finds the first attribute associated with the provided <see cref="Enum" /> and allows retrieval of a value from it.
    /// </summary>
    /// <param name="value">The enum value whose attribute data is to be obtained.</param>
    /// <param name="valueTask">The logic to perform that will return a value.</param>
    /// <returns></returns>
    public static TResult? ToAttributeValue<TAttribute, TResult>(this Enum value, Func<TAttribute, TResult> valueTask)
        where TAttribute : Attribute
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo?.GetCustomAttributes(typeof(TAttribute), false) is TAttribute[] attributes && attributes.Any())
        {
            return valueTask(attributes.First());
        }

        return default;
    }

    /// <summary>
    ///     Gets the value that is in the <see cref="DescriptionAttribute" /> for a specific <see cref="Enum" /> value.
    /// </summary>
    /// <param name="value">The enum value whose description is to be obtained.</param>
    /// <returns>The value of the enum's description, otherwise null.</returns>
    public static string? ToDescriptionValue(this Enum value)
    {
        return value.ToAttributeValue<DescriptionAttribute, string?>(x => x.Description);
    }
}
