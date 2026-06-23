using FluentResults;

namespace MLib3.MVVM.Navigation;

/// <summary>
/// Provides navigation functionalities for view models.
/// </summary>
/// <remarks>
/// This interface extends <see cref="INavigator{IViewModel}"/> and adds methods that leverage 
/// dependency injection to resolve ViewModels, which is particularly useful for handling 
/// circular dependencies between ViewModels.
/// </remarks>
public interface INavigator : INavigator<IViewModel>
{
    /// <summary>
    /// Pushes a new ViewModel of type <typeparamref name="TViewModel"/> onto the navigation stack.
    /// The ViewModel is resolved using the registered <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the ViewModel to push.</typeparam>
    /// <returns>A <see cref="Result{TViewModel}"/> containing the resolved ViewModel if successful; otherwise, an error.</returns>
    Result<TViewModel> Push<TViewModel>() where TViewModel : IViewModel;

    /// <summary>
    /// Navigates to a new ViewModel of type <typeparamref name="TViewModel"/> and clears the navigation stack.
    /// The ViewModel is resolved using the registered <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the ViewModel to navigate to.</typeparam>
    /// <returns>A <see cref="Result{TViewModel}"/> containing the resolved ViewModel if successful; otherwise, an error.</returns>
    Result<TViewModel> Goto<TViewModel>() where TViewModel : IViewModel;

    /// <summary>
    /// Replaces the current ViewModel with a new ViewModel of type <typeparamref name="TViewModel"/>.
    /// The ViewModel is resolved using the registered <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the ViewModel to slide to.</typeparam>
    /// <returns>A <see cref="Result{TViewModel}"/> containing the resolved ViewModel if successful; otherwise, an error.</returns>
    Result<TViewModel> Slide<TViewModel>() where TViewModel : IViewModel;
}