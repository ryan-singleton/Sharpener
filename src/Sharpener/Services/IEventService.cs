// The Sharpener project licenses this file to you under the MIT license.

namespace Sharpener.Services;

/// <summary>
///     Contract for an event aggregator that allows subscribing to, unsubscribing from, and raising events by type and
///     optional key.
/// </summary>
public interface IEventService
{
    /// <summary>
    ///     Subscribes a handler to the specified event type and optional key.
    /// </summary>
    /// <typeparam name="T">The type of event arguments for the event.</typeparam>
    /// <param name="handler">The action to invoke when the event is raised.</param>
    /// <param name="key">An optional string key to distinguish multiple events of the same type.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="handler" /> is null.</exception>
    void Subscribe<T>(Action<T> handler, string key = "") where T : EventArgs;

    /// <summary>
    ///     Unsubscribes a handler from the specified event type and optional key.
    /// </summary>
    /// <typeparam name="T">The type of event arguments for the event.</typeparam>
    /// <param name="handler">The action to remove from the event subscription list.</param>
    /// <param name="key">An optional string key to distinguish multiple events of the same type.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="handler" /> is null.</exception>
    void Unsubscribe<T>(Action<T> handler, string key = "") where T : EventArgs;

    /// <summary>
    ///     Raises the specified event, invoking all subscribed handlers for the given type and key.
    /// </summary>
    /// <typeparam name="T">The type of event arguments for the event.</typeparam>
    /// <param name="args">
    ///     The event arguments to pass to handlers. If null, a new instance of <typeparamref name="T" /> will be created.
    /// </param>
    /// <param name="key">An optional string key to distinguish multiple events of the same type.</param>
    /// <param name="onFail">
    ///     If the raising of the event throws an exception, it will be ignored unless an action is provided
    ///     here.
    /// </param>
    void Raise<T>(T? args = null, string key = "", Action<Exception>? onFail = null) where T : EventArgs;
}
