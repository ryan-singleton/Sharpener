// The Sharpener project licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sharpener.Extensions;

namespace Sharpener.Rest.Extensions;

/// <summary>
///     Extensions for dependency injection.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    ///     Finds every type that is assignable from the specified generic type. This includes derived classes or
    ///     implementations if the generic type is an interface. The discovered types are then registered in the dependency
    ///     injection container.
    /// </summary>
    /// <param name="services">The service collection to register the discovered types to.</param>
    /// <param name="registrationAction">
    ///     The registration action to take place. Optional. By default, this calls TryAddScoped,
    ///     but can be adjusted.
    /// </param>
    /// <param name="scanSystemAssemblies">Whether to scan System or Microsoft assemblies. Default is false.</param>
    /// <typeparam name="T">The type whose assignable references should be registered.</typeparam>
    /// <returns>The <see cref="IServiceCollection" /> with all registrations performed.</returns>
    public static IServiceCollection TryAddWhereAssignableFrom<T>(this IServiceCollection services,
        Action<IServiceCollection, Type>? registrationAction = null, bool scanSystemAssemblies = false)
    {
        registrationAction ??= (s, t) => s.TryAddScoped(t);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic);

        if (!scanSystemAssemblies)
        {
            var systemAssemblyPrefixes = new[] { "System", "Microsoft" };
            assemblies = assemblies.Where(assembly =>
                systemAssemblyPrefixes.All(prefix => assembly.FullName?.NoCase().StartsWith(prefix) != true));
        }

        var types = assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsAbstract);
        types.ForAll(type => registrationAction?.Invoke(services, type));
        return services;
    }
}
