// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Injection;

/// <summary>
///     The contract for a lightweight, minimal purpose dependency injection resolver.
/// </summary>
public interface IServiceResolver
{
    /// <summary>
    ///     Gets all registered names for a specific service type.
    /// </summary>
    /// <typeparam name="T">The type to get names for.</typeparam>
    /// <returns>A collection of all registered names for the specified type.</returns>
    IEnumerable<string> GetRegisteredNames<T>();

    /// <summary>
    ///     Determines whether a service type is registered.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    /// <returns><c>true</c> if the type is registered; otherwise, <c>false</c>.</returns>
    bool IsRegistered<T>();

    /// <summary>
    ///     Determines whether a named service type is registered.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    /// <param name="name">The name to check.</param>
    /// <returns><c>true</c> if the named type is registered; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the name is null or empty.</exception>
    bool IsRegistered<T>(string name);

    /// <summary>
    ///     Registers a transient service type and constructs an instance of it with registered services.
    /// </summary>
    /// <typeparam name="TService">The interface or base type.</typeparam>
    /// <param name="overwriteExisting">Whether to overwrite an existing registration.</param>
    /// <param name="configure">The optional logic to run once the service is resolved.</param>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    IServiceResolver Register<TService>(bool overwriteExisting = false, Action<TService>? configure = null)
        where TService : class;

    /// <summary>
    ///     Registers a transient service type and its implementation.
    /// </summary>
    /// <typeparam name="TService">The interface or base type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
    /// <param name="overwriteExisting">Whether to overwrite an existing registration.</param>
    /// <param name="configure">The optional logic to run once the service is resolved.</param>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    IServiceResolver Register<TService, TImplementation>(bool overwriteExisting = false,
        Action<TService>? configure = null)
        where TImplementation : class, TService where TService : class;

    /// <summary>
    ///     Registers a named transient service type and its implementation.
    /// </summary>
    /// <typeparam name="TService">The interface or base type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
    /// <param name="name">The name for this registration.</param>
    /// <param name="overwriteExisting">Whether to overwrite an existing registration.</param>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    /// <param name="configure">The optional logic to run once the service is resolved.</param>
    /// <exception cref="ArgumentNullException">Thrown if the name is null or empty.</exception>
    IServiceResolver Register<TService, TImplementation>(string name, bool overwriteExisting = false,
        Action<TService>? configure = null)
        where TImplementation : class, TService where TService : class;

    /// <summary>
    ///     Registers a singleton instance of a service by generating an instance of it with registered services.
    /// </summary>
    /// <typeparam name="TService">The interface or base type.</typeparam>
    /// <param name="overwriteExisting">Whether to overwrite an existing registration.</param>
    /// <param name="configure">The optional logic to run on the service before registering it.</param>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided instance is null.</exception>
    IServiceResolver RegisterSingleton<TService>(bool overwriteExisting = false, Action<TService>? configure = null)
        where TService : class;

    /// <summary>
    ///     Registers a singleton instance of a service by generating an instance of it with registered services.
    /// </summary>
    /// <typeparam name="TService">The interface or base type.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
    /// <param name="overwriteExisting">Whether to overwrite an existing registration.</param>
    /// <param name="configure">The optional logic to run on the service before registering it.</param>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided instance is null.</exception>
    IServiceResolver RegisterSingleton<TService, TImplementation>(bool overwriteExisting = false,
        Action<TService>? configure = null) where TImplementation : class, TService where TService : class;

    /// <summary>
    ///     Registers a singleton instance of a service.
    /// </summary>
    /// <typeparam name="TService">The interface or base type.</typeparam>
    /// <param name="instance">The instance to register.</param>
    /// <param name="overwriteExisting">Whether to overwrite an existing registration.</param>
    /// <param name="configure">The optional logic to run on the service before registering it.</param>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided instance is null.</exception>
    IServiceResolver RegisterSingleton<TService>(TService instance, bool overwriteExisting = false,
        Action<TService>? configure = null) where TService : class;

    /// <summary>
    ///     Registers a named singleton instance of a service.
    /// </summary>
    /// <typeparam name="TService">The interface or base type.</typeparam>
    /// <param name="name">The name for this registration.</param>
    /// <param name="instance">The instance to register.</param>
    /// <param name="overwriteExisting">Whether to overwrite an existing registration.</param>
    /// <param name="configure">The optional logic to run on the service before registering it.</param>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the name is null or empty, or if the provided instance is null.</exception>
    IServiceResolver RegisterSingleton<TService>(string name, TService instance, bool overwriteExisting = false,
        Action<TService>? configure = null) where TService : class;

    /// <summary>
    ///     Resolves an instance of the specified service type.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <returns>An instance of the requested type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no registration exists for the requested type.</exception>
    T Resolve<T>();

    /// <summary>
    ///     Resolves a named instance of the specified service type.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <param name="name">The name of the registration.</param>
    /// <returns>An instance of the requested type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no named registration exists for the requested type.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the name is null or empty.</exception>
    T Resolve<T>(string name);

    /// <summary>
    ///     Sets the service resolver as the globally accessible current service resolver in <see cref="Resolvers.Current" />.
    ///     Note that this can only be called once, so it is important to configure the resolver to a final state before
    ///     calling this for the rest of the application lifetime.
    /// </summary>
    /// <returns>The current <see cref="IServiceResolver" /> for fluent configuration.</returns>
    IServiceResolver SetCurrent();

    /// <summary>
    ///     Returns all registered services that implement the specified interface or base type.
    /// </summary>
    /// <typeparam name="T">The interface or base type to match.</typeparam>
    /// <returns>A list of registered service instances implementing or deriving from <typeparamref name="T" />.</returns>
    List<T> WhereAssignableFrom<T>();
}
