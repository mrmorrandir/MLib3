using MLib3.MVVM;
using MLib3.MVVM.Navigation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    /// <summary>
    /// Adds navigation services to the dependency injection container for a specific ViewModel type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method registers <see cref="INavigator{T}"/> which requires ViewModels to be provided manually 
    /// when calling navigation methods (e.g., <c>Push(T viewModel)</c>).
    /// </para>
    /// </remarks>
    /// <param name="services">The dependency injection service collection to which the navigation services will be added.</param>
    /// <typeparam name="T">The type of the view model that the navigator will manage. This type must implement <see cref="IViewModel"/>.</typeparam>
    /// <returns>The updated instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddNavigation<T>(this IServiceCollection services) where T : IViewModel
    {
        services.AddSingleton<INavigator<T>, Navigator<T>>();
        return services;
    }

    /// <summary>
    /// Adds navigation services for non-generic view models to the dependency injection container.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method registers the non-generic <see cref="INavigator"/> interface. 
    /// The <see cref="Navigator"/> implementation uses <see cref="IServiceProvider"/> to resolve ViewModels.
    /// </para>
    /// <para>
    /// Using this method is recommended when you have circular dependencies between ViewModels 
    /// (e.g., ViewModel A navigates to ViewModel B, and ViewModel B navigates back to ViewModel A).
    /// By using <c>navigator.Push&lt;TViewModel&gt;()</c>, the ViewModel is resolved from the DI container 
    /// at runtime, breaking the constructor injection cycle.
    /// </para>
    /// </remarks>
    /// <param name="services">The dependency injection service collection to which the navigation services will be added.</param>
    /// <returns>The updated instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddNavigation(this IServiceCollection services)
    {
        services.AddNavigation<IViewModel>();
        services.AddSingleton<INavigator, Navigator>();
        return services;
    }
}