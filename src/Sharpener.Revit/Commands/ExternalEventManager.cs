// The Sharpener project licenses this file to you under the MIT license.

using Autodesk.Revit.UI;

namespace Sharpener.Revit.Commands;

/// <summary>
///     Manages the lifecycle, registration, invocation, and disposal of external events
///     within the context of Revit's External Event API.
/// </summary>
public class ExternalEventManager : IDisposable, IExternalEventManager
{
    private readonly Dictionary<Type, (ExternalEvent Event, IExternalEventHandler Handler)> _externalEvents = [];
    private bool _disposed;

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        UnregisterAllEvents();
        _disposed = true;
    }

    /// <inheritdoc />
    public T? GetHandler<T>() where T : class, IExternalEventHandler
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ExternalEventManager));
        }

        var handlerType = typeof(T);
        return _externalEvents.TryGetValue(handlerType, out var eventData)
            ? eventData.Handler as T
            : null;
    }

    /// <inheritdoc />
    public void RaiseEvent<T>() where T : class, IExternalEventHandler
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ExternalEventManager));
        }

        var handlerType = typeof(T);
        if (_externalEvents.TryGetValue(handlerType, out var eventData))
        {
            eventData.Event.Raise();
        }
    }

    /// <inheritdoc />
    public void RegisterEvent<T>(T handler) where T : class, IExternalEventHandler
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ExternalEventManager));
        }

        var handlerType = typeof(T);
        if (_externalEvents.ContainsKey(handlerType))
        {
            throw new InvalidOperationException($"Event handler of type '{handlerType.Name}' is already registered.");
        }

        var externalEvent = ExternalEvent.Create(handler);
        _externalEvents[handlerType] = (externalEvent, handler);
    }

    /// <inheritdoc />
    public void UnregisterAllEvents()
    {
        if (_disposed)
        {
            return;
        }

        foreach (var pair in _externalEvents)
        {
            pair.Value.Event.Dispose();
        }

        _externalEvents.Clear();
    }

    /// <inheritdoc />
    public void UnregisterEvent<T>() where T : class, IExternalEventHandler
    {
        if (_disposed)
        {
            return;
        }

        var handlerType = typeof(T);
        if (!_externalEvents.TryGetValue(handlerType, out var eventData))
        {
            return;
        }

        if (!_externalEvents.Remove(handlerType))
        {
            return;
        }

        eventData.Event.Dispose();
    }
}
