// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Commands;

/// <summary>
///     Defines methods for managing external events in Revit, including registering, unregistering,
///     and triggering events through their associated handlers.
/// </summary>
public interface IExternalEventManager
{
    /// <summary>
    ///     Retrieves the registered external event handler of the specified type if it exists.
    /// </summary>
    /// <typeparam name="T">The type of the event handler implementing <see cref="IExternalEventHandler" />.</typeparam>
    /// <returns>
    ///     The instance of the registered external event handler for the specified type, or null if no handler is
    ///     registered.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown when the method is called on a disposed instance of <see cref="ExternalEventManager" />.
    /// </exception>
    T? GetHandler<T>() where T : class, IExternalEventHandler;

    /// <summary>
    ///     Triggers the external event associated with the specified handler type within the event manager.
    /// </summary>
    /// <typeparam name="T">The type of the external event handler implementing <see cref="IExternalEventHandler" />.</typeparam>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown when the method is called on a disposed instance of <see cref="ExternalEventManager" />.
    /// </exception>
    /// <remarks>
    ///     This method invokes the Raise method of the associated <see cref="ExternalEvent" /> for the specified handler type.
    ///     If the handler type has not been registered, no action is taken.
    /// </remarks>
    void RaiseEvent<T>() where T : class, IExternalEventHandler;

    /// <summary>
    ///     Registers an external event handler of the specified type within the event manager.
    /// </summary>
    /// <typeparam name="T">The type of the event handler implementing <see cref="IExternalEventHandler" />.</typeparam>
    /// <param name="handler">The instance of the event handler to be registered.</param>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown when the method is called on a disposed instance of <see cref="ExternalEventManager" />.
    /// </exception>
    void RegisterEvent<T>(T handler) where T : class, IExternalEventHandler;

    /// <summary>
    ///     Unregisters all external event handlers managed by the event manager and disposes their associated events.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if the method is invoked on a disposed instance of <see cref="ExternalEventManager" />.
    /// </exception>
    void UnregisterAllEvents();

    /// <summary>
    ///     Unregisters the external event handler of the specified type from the event manager and disposes of its associated
    ///     external event.
    /// </summary>
    /// <typeparam name="T">The type of the event handler implementing <see cref="IExternalEventHandler" />.</typeparam>
    /// <remarks>
    ///     If the event handler has not been registered, the method performs no action.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if the method is called on a disposed instance of <see cref="ExternalEventManager" />.
    /// </exception>
    void UnregisterEvent<T>() where T : class, IExternalEventHandler;
}
