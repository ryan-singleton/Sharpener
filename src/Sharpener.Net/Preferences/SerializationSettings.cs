using Sharpener.Net.Delegates;
using System.Text.Json;

namespace Sharpener.Net.Preferences;

/// <summary>
/// Settings for serialization that can be customized on a per-application basis.
/// </summary>
/// <remarks>
/// If you find yourself recreating the same preferences in each new application, consider making a library or package to more easily share them.
/// </remarks>
public static class SerializationSettings
{
    private static DelegateStore<Func<object, string>> _toJsonDelegates = default!;

    private static DelegateStore<Func<string, Type, object?>> _fromJsonDelegates = default!;


    /// <summary>
    /// Constructor.
    /// </summary>
    static SerializationSettings()
    {
        Reset();
    }

    /// <summary>
    /// Resets all of the delegates to their default state. All custom delegates will be cleared and only ordinal, invariant, and current culture with varying case sensitivity will be present again.
    /// </summary>
    public static void Reset()
    {
        _toJsonDelegates = new DelegateStore<Func<object, string>>(model => JsonSerializer.Serialize(model));
        _toJsonDelegates.SetNamed("standard", model => JsonSerializer.Serialize(model));
        _toJsonDelegates.SetNamed("indented", model => JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true }));

        _fromJsonDelegates = new DelegateStore<Func<string, Type, object?>>((json, type) => JsonSerializer.Deserialize(json, type));
        _fromJsonDelegates.SetNamed("standard", (json, type) => JsonSerializer.Deserialize(json, type));
    }

    /// <summary>
    /// Gets the default registered FromJson logic.
    /// </summary>
    public static Func<string, Type, object?> DefaultFromJson => _fromJsonDelegates.Default;

    /// <summary>
    /// Gets the default registered ToJson logic.
    /// </summary>
    public static Func<object, string> DefaultToJson => _toJsonDelegates.Default;

    /// <summary>
    /// Gets ToJson logic by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Func<object, string>? GetNamedToJson(string name) => _toJsonDelegates.GetNamed(name);

    /// <summary>
    /// Gets FromJson logic by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Func<string, Type, object?>? GetNamedFromJson(string name) => _fromJsonDelegates.GetNamed(name);

    /// <summary>
    /// Sets default ToJson logic. Will throw if logic is null!
    /// </summary>
    /// <param name="logic"></param>
    public static void SetDefaultToJson(Func<object, string> logic) => _toJsonDelegates.SetDefault(logic);

    /// <summary>
    /// Sets default FromJson logic. Will throw if logic is null!
    /// </summary>
    /// <param name="logic"></param>
    public static void SetDefaultFromJson(Func<string, Type, object?> logic) => _fromJsonDelegates.SetDefault(logic);

    /// <summary>
    /// Sets ToJson logic by name.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="logic"></param>
    public static void SetNamedToJson(string name, Func<object, string> logic) => _toJsonDelegates.SetNamed(name, logic);

    /// <summary>
    /// Sets FromJson logic by name.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="logic"></param>
    public static void SetNamedFromJson(string name, Func<string, Type, object?> logic) => _fromJsonDelegates.SetNamed(name, logic);
}
