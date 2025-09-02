// The Sharpener project licenses this file to you under the MIT license.

using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace Sharpener.Injection;

/// <summary>
///     A lightweight dependency injection container that supports constructor injection, singleton registration, and named
///     services.
/// </summary>
public class ServiceResolver : IServiceResolver
{
    private readonly ConcurrentDictionary<(Type, string), Func<object>> _namedRegistrations = new();
    private readonly ConcurrentDictionary<Type, Func<object>> _registrations = new();

    /// <inheritdoc />
    public IEnumerable<string> GetRegisteredNames<T>()
    {
        var type = typeof(T);
        return _namedRegistrations.Keys
            .Where(k => k.Item1 == type)
            .Select(k => k.Item2);
    }

    /// <inheritdoc />
    public bool IsRegistered<T>()
    {
        return _registrations.ContainsKey(typeof(T));
    }

    /// <inheritdoc />
    public bool IsRegistered<T>(string name)
    {
        ThrowIfNullOrEmpty(name);
        return _namedRegistrations.ContainsKey((typeof(T), name));
    }

    /// <inheritdoc />
    public IServiceResolver Register<TService>(bool overwriteExisting = false, Action<TService>? configure = null)
        where TService : class
    {
        return RegisterInternal<TService, TService>(overwriteExisting, configure, true);
    }

    /// <inheritdoc />
    public IServiceResolver Register<TService, TImplementation>(bool overwriteExisting = false,
        Action<TService>? configure = null)
        where TImplementation : class, TService where TService : class
    {
        return RegisterInternal<TService, TImplementation>(overwriteExisting, configure, true);
    }

    /// <inheritdoc />
    public IServiceResolver Register<TService, TImplementation>(string name, bool overwriteExisting = false,
        Action<TService>? configure = null)
        where TImplementation : class, TService where TService : class
    {
        return RegisterNamedInternal<TService, TImplementation>(name, overwriteExisting, configure, true);
    }

    /// <inheritdoc />
    public IServiceResolver RegisterSingleton<TService>(bool overwriteExisting = false,
        Action<TService>? configure = null) where TService : class
    {
        return RegisterInternal<TService, TService>(overwriteExisting, configure, false);
    }

    /// <inheritdoc />
    public IServiceResolver RegisterSingleton<TService, TImplementation>(bool overwriteExisting = false,
        Action<TService>? configure = null)
        where TImplementation : class, TService where TService : class
    {
        return RegisterInternal<TService, TImplementation>(overwriteExisting, configure, false);
    }

    /// <inheritdoc />
    public IServiceResolver RegisterSingleton<TService>(TService instance, bool overwriteExisting = false,
        Action<TService>? configure = null)
        where TService : class
    {
        if (IsRegistered<TService>() && !overwriteExisting)
        {
            return this;
        }

        ThrowIfNull(instance);
        configure?.Invoke(instance);

        _registrations[typeof(TService)] = () => instance;
        return this;
    }

    /// <inheritdoc />
    public IServiceResolver RegisterSingleton<TService>(string name, TService instance, bool overwriteExisting = false,
        Action<TService>? configure = null)
        where TService : class
    {
        ThrowIfNullOrEmpty(name);
        ThrowIfNull(instance);

        if (IsRegistered<TService>(name) && !overwriteExisting)
        {
            return this;
        }

        configure?.Invoke(instance);
        _namedRegistrations[(typeof(TService), name)] = () => instance;
        return this;
    }

    /// <inheritdoc />
    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }

    /// <inheritdoc />
    public T Resolve<T>(string name)
    {
        ThrowIfNullOrEmpty(name);
        return (T)ResolveNamed(typeof(T), name);
    }

    /// <inheritdoc />
    public IServiceResolver SetCurrent()
    {
        Resolvers.SetCurrent(this);
        return this;
    }

    /// <inheritdoc />
    public List<T> WhereAssignableFrom<T>()
    {
        var targetType = typeof(T);
        var results = new List<T>();

        foreach (var registration in _registrations)
        {
            var registeredType = registration.Key;
            if (targetType.IsAssignableFrom(registeredType) && Resolve(registeredType) is T instance)
            {
                results.Add(instance);
            }
        }

        foreach (var registration in _namedRegistrations)
        {
            var registeredType = registration.Key.Item1;
            var name = registration.Key.Item2;
            if (targetType.IsAssignableFrom(registeredType) && ResolveNamed(registeredType, name) is T instance)
            {
                results.Add(instance);
            }
        }

        return results;
    }

    private static object ConvertToCollectionType(IEnumerable<object> services, Type targetType, Type elementType)
    {
        var servicesList = services.ToList();

        if (targetType.IsArray)
        {
            var array = Array.CreateInstance(elementType, servicesList.Count);
            for (var i = 0; i < servicesList.Count; i++)
            {
                array.SetValue(servicesList[i], i);
            }

            return array;
        }

        if (!targetType.IsGenericType)
        {
            return servicesList;
        }

        var genericTypeDef = targetType.GetGenericTypeDefinition();

        if (genericTypeDef == typeof(IEnumerable<>) ||
            genericTypeDef == typeof(ICollection<>) ||
            genericTypeDef == typeof(IList<>) ||
            genericTypeDef == typeof(IReadOnlyCollection<>) ||
            genericTypeDef == typeof(IReadOnlyList<>))
        {
            return CreateTypedList(servicesList, elementType);
        }

        return genericTypeDef == typeof(List<>) ? CreateTypedList(servicesList, elementType) : servicesList;
    }

    private TService CreateAndConfigure<TService, TImplementation>(Action<TService>? configure)
        where TImplementation : class, TService where TService : class
    {
        var instance = (TService)CreateInstance(typeof(TImplementation));
        configure?.Invoke(instance);
        return instance;
    }

    private object CreateInstance(Type type)
    {
        var constructors = type.GetConstructors();

        foreach (var constructor in constructors.OrderByDescending(c => c.GetParameters().Length))
        {
            if (TryResolveConstructor(constructor, out var args))
            {
                return constructor.Invoke(args);
            }
        }

        return Activator.CreateInstance(type)
               ?? throw new InvalidOperationException($"Could not create instance of {type.FullName}.");
    }

    private static object CreateTypedList(List<object> services, Type elementType)
    {
        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = Activator.CreateInstance(listType)!;
        var addMethod = listType.GetMethod("Add")!;

        foreach (var service in services)
        {
            addMethod.Invoke(list, [service]);
        }

        return list;
    }

    private static bool IsCollectionType(Type type, out Type elementType)
    {
        elementType = null!;

        if (type.IsArray)
        {
            elementType = type.GetElementType()!;
            return true;
        }

        if (type.IsGenericType)
        {
            var genericTypeDef = type.GetGenericTypeDefinition();

            if (genericTypeDef == typeof(IEnumerable<>) ||
                genericTypeDef == typeof(ICollection<>) ||
                genericTypeDef == typeof(IList<>) ||
                genericTypeDef == typeof(List<>) ||
                genericTypeDef == typeof(IReadOnlyCollection<>) ||
                genericTypeDef == typeof(IReadOnlyList<>))
            {
                elementType = type.GetGenericArguments()[0];
                return true;
            }
        }

        if (type != typeof(IEnumerable))
        {
            return false;
        }

        elementType = typeof(object);
        return true;
    }

    private ServiceResolver RegisterInternal<TService, TImplementation>(bool overwriteExisting,
        Action<TService>? configure, bool isTransient) where TImplementation : class, TService where TService : class
    {
        if (IsRegistered<TService>() && !overwriteExisting)
        {
            return this;
        }

        if (isTransient)
        {
            _registrations[typeof(TService)] = () => CreateAndConfigure<TService, TImplementation>(configure);
            return this;
        }

        var instance = CreateAndConfigure<TService, TImplementation>(configure);
        _registrations[typeof(TService)] = () => instance;

        return this;
    }

    private ServiceResolver RegisterNamedInternal<TService, TImplementation>(string name, bool overwriteExisting,
        Action<TService>? configure, bool isTransient) where TImplementation : class, TService where TService : class
    {
        ThrowIfNullOrEmpty(name);

        if (IsRegistered<TService>(name) && !overwriteExisting)
        {
            return this;
        }

        if (isTransient)
        {
            _namedRegistrations[(typeof(TService), name)] =
                () => CreateAndConfigure<TService, TImplementation>(configure);
            return this;
        }

        var instance = CreateAndConfigure<TService, TImplementation>(configure);
        _namedRegistrations[(typeof(TService), name)] = () => instance;

        return this;
    }

    private object Resolve(Type type)
    {
        if (!_registrations.TryGetValue(type, out var factory))
        {
            throw new InvalidOperationException($"No registration found for type {type}.");
        }

        return factory();
    }

    private object ResolveNamed(Type type, string name)
    {
        if (!_namedRegistrations.TryGetValue((type, name), out var factory))
        {
            throw new InvalidOperationException($"No named registration found for type {type} with name '{name}'.");
        }

        return factory();
    }

    private static void ThrowIfNull(object obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
    }

    private static void ThrowIfNullOrEmpty(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
    }

    private bool TryResolveAsCollection(Type paramType, out object collectionValue)
    {
        collectionValue = null!;
        if (!IsCollectionType(paramType, out var elementType))
        {
            return false;
        }

        var compatibleServices = WhereAssignableFrom(elementType);
        collectionValue = ConvertToCollectionType(compatibleServices, paramType, elementType);
        return true;
    }

    private bool TryResolveConstructor(ConstructorInfo constructor, out object[] args)
    {
        var parameters = constructor.GetParameters();
        args = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var paramType = parameters[i].ParameterType;

            if (TryResolveAsCollection(paramType, out var collectionValue))
            {
                args[i] = collectionValue;
                continue;
            }

            if (!_registrations.ContainsKey(paramType))
            {
                return false;
            }

            args[i] = Resolve(paramType);
        }

        return true;
    }

    private IEnumerable<object> WhereAssignableFrom(Type baseType)
    {
        return _registrations
            .Where(kvp => baseType.IsAssignableFrom(kvp.Key))
            .Select(kvp => Resolve(kvp.Key));
    }
}
