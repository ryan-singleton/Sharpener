// The Sharpener project licenses this file to you under the MIT license.

using System.Collections.Concurrent;

namespace Sharpener.Services;

/// <summary>
///     Provides a lightweight event aggregator that allows subscribing to, unsubscribing from, and raising events by type
///     and optional key.
/// </summary>
public class EventService : IEventService
{
    private readonly ConcurrentDictionary<(Type, string), List<Delegate>> _handlers = new();
    private readonly object _lock = new();

    /// <inheritdoc />
    public void Subscribe<T>(Action<T> handler, string key = "") where T : EventArgs
    {
        if (handler is null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        var eventKey = (typeof(T), key);

        lock (_lock)
        {
            if (!_handlers.TryGetValue(eventKey, out var handlers))
            {
                handlers = [];
                _handlers[eventKey] = handlers;
            }

            handlers.Add(handler);
        }
    }

    /// <inheritdoc />
    public void Unsubscribe<T>(Action<T> handler, string key = "") where T : EventArgs
    {
        if (handler is null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        var eventKey = (typeof(T), key);

        lock (_lock)
        {
            if (!_handlers.TryGetValue(eventKey, out var handlers))
            {
                return;
            }

            handlers.Remove(handler);
            if (handlers.Count == 0)
            {
                _handlers.TryRemove(eventKey, out _);
            }
        }
    }

    /// <inheritdoc />
    public void Raise<T>(T? args = null, string key = "", Action<Exception>? onFail = null) where T : EventArgs
    {
        var eventKey = (typeof(T), key);
        List<Delegate>? handlersCopy = null;

        lock (_lock)
        {
            if (_handlers.TryGetValue(eventKey, out var handlers) && handlers.Count > 0)
            {
                handlersCopy = [.. handlers];
            }
        }

        if (handlersCopy is null)
        {
            return;
        }

        args ??= Activator.CreateInstance<T>();

        foreach (var handler in handlersCopy)
        {
            try
            {
                ((Action<T>)handler).Invoke(args);
            }
            catch (Exception ex)
            {
                onFail?.Invoke(ex);
            }
        }
    }
}
