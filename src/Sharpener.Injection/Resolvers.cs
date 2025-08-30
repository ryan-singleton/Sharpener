// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Injection;

/// <summary>
///     For situations where the resolver must be referred to statically. In projects that deal with unique contexts like
///     software API's and external events particularly. This is where globally accessible containers can be stored.
/// </summary>
public static class Resolvers
{
    private static IServiceResolver? _current;

    /// <summary>
    ///     Lazily initialized service resolver. The instance is only created when first accessed.
    ///     Thread-safe and guarantees only one instance is created.
    /// </summary>
    public static IServiceResolver Current => _current!;

    /// <summary>
    ///     Checks if the Current service resolver has been initialized.
    /// </summary>
    public static bool IsInitialized => _current is not null;

    /// <summary>
    ///     Resolves an instance of the specified type using the current service resolver.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <returns>An instance of the requested type.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the resolver has not been initialized or if no registration
    ///     exists for the requested type.
    /// </exception>
    public static T Resolve<T>() where T : class
    {
        return Current.Resolve<T>();
    }

    /// <summary>
    ///     Allows you to replace the default service resolver with a pre-configured one.
    ///     This must be called before any access to Current, otherwise it will throw an exception.
    /// </summary>
    /// <param name="serviceResolver">The service resolver to use as the global instance</param>
    /// <param name="allowReset">Whether the current resolver can be reset. Defaults to false.</param>
    /// <exception cref="InvalidOperationException">Thrown if Current has already been accessed</exception>
    public static void SetCurrent(this IServiceResolver serviceResolver, bool allowReset = false)
    {
        if (IsInitialized && !allowReset)
        {
            throw new InvalidOperationException(
                "Cannot set Current after it has already been accessed. Call SetCurrent before any access to Current.");
        }

        _current = serviceResolver ?? throw new ArgumentNullException(nameof(serviceResolver));
    }
}
