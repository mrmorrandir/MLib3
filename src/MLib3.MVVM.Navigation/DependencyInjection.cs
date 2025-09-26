using MLib3.MVVM;
using MLib3.MVVM.Navigation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddNavigation<T>(this IServiceCollection services) where T : IViewModel
    {
        services.AddSingleton<INavigator<T>, Navigator<T>>();
        return services;
    }
}