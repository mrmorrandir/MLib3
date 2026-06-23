using System.ComponentModel;

namespace MLib3.MVVM.Navigation;

/// <summary>
/// Provides navigation functionalities for view models of type <typeparamref name="T"/>.
/// </summary>
/// <remarks>
/// This interface requires ViewModels to be provided explicitly for navigation operations.
/// </remarks>
/// <typeparam name="T">The base type of ViewModels managed by this navigator.</typeparam>
public partial interface INavigator<T> where T : IViewModel
{
    /// <summary>
    /// Gets an observable that notifies when the navigation has changed.
    /// </summary>
    IObservable<NavigationChangedInfo<T>> NavigationChanged { get; }

    /// <summary>
    /// Gets the current ViewModel.
    /// </summary>
    T CurrentViewModel { get; }

    /// <summary>
    /// Gets an observable that notifies when a ViewModel has been reached (navigated to).
    /// </summary>
    IObservable<T> Reached { get; }

    /// <summary>
    /// Gets an observable that notifies when a ViewModel has been left (navigated away from).
    /// </summary>
    IObservable<T> Left { get; }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Pushes the specified ViewModel onto the navigation stack.
    /// </summary>
    /// <param name="viewModel">The ViewModel to push.</param>
    void Push(T viewModel);

    /// <summary>
    /// Pops the current ViewModel from the navigation stack.
    /// </summary>
    void Pop();

    /// <summary>
    /// Navigates to the specified ViewModel and clears the navigation stack.
    /// </summary>
    /// <param name="viewModel">The ViewModel to navigate to.</param>
    void Goto(T viewModel);

    /// <summary>
    /// Replaces the current ViewModel with the specified ViewModel.
    /// </summary>
    /// <param name="viewModel">The ViewModel to slide to.</param>
    void Slide(T viewModel);
}