using System.ComponentModel;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace MLib3.MVVM.Navigation;

/// <summary>
///     Provides navigation functionalities for view models within the application.
///     Implements the <see cref="INavigator" /> interface and leverages dependency injection
///     to manage navigation transitions between different view models.
/// </summary>
/// <remarks>
///     <para>
///         This <see cref="Navigator" /> wraps the <see cref="INavigator{T}" /> interface,
///         allowing for navigation operations to be performed within the application.
///         It leverages dependency injection to resolve view models and manage navigation transitions.
///     </para>
///     <para>
///         The usage of the <see cref="IServiceProvider" /> is crucial for resolving view models without circular
///         dependencies.
///     </para>
///     <para>
///         The class is marked as <c>partial</c> to allow developers to extend it with domain-specific
///         navigation methods like <c>GotoSettings()</c> or <c>PushProductDetails(vm)</c>.
///         Custom methods should focus strictly on navigation transitions and avoid business logic.
///     </para>
/// </remarks>
public partial class Navigator : INavigator
{
    private readonly INavigator<IViewModel> _navigator;
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    public IObservable<NavigationChangedInfo<IViewModel>> NavigationChanged => _navigator.NavigationChanged;
    /// <inheritdoc />
    public IViewModel CurrentViewModel => _navigator.CurrentViewModel;
    /// <inheritdoc />
    public IObservable<IViewModel> Reached => _navigator.Reached;
    /// <inheritdoc />
    public IObservable<IViewModel> Left => _navigator.Left;

    /// <summary>
    /// Initializes a new instance of the <see cref="Navigator"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve ViewModels.</param>
    /// <param name="navigator">The underlying generic navigator.</param>
    public Navigator(IServiceProvider serviceProvider, INavigator<IViewModel> navigator)
    {
        _serviceProvider = serviceProvider;
        _navigator = navigator;
    }

    /// <inheritdoc />
    public void Push(IViewModel viewModel)
    {
        _navigator.Push(viewModel);
    }

    /// <inheritdoc />
    public void Pop()
    {
        _navigator.Pop();
    }

    /// <inheritdoc />
    public void Goto(IViewModel viewModel)
    {
        _navigator.Goto(viewModel);
    }

    /// <inheritdoc />
    public void Slide(IViewModel viewModel)
    {
        _navigator.Slide(viewModel);
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => _navigator.PropertyChanged += value;
        remove => _navigator.PropertyChanged -= value;
    }

    /// <summary>
    /// Pushes a new ViewModel of type <typeparamref name="TViewModel"/> onto the navigation stack.
    /// </summary>
    /// <remarks>
    /// This method resolves the ViewModel from the DI container, which avoids circular dependency issues
    /// in constructors.
    /// </remarks>
    /// <typeparam name="TViewModel">The type of the ViewModel to push.</typeparam>
    /// <returns>A <see cref="Result{TViewModel}"/> containing the resolved ViewModel if successful; otherwise, an error.</returns>
    public Result<TViewModel> Push<TViewModel>() where TViewModel : IViewModel
    {
        var viewModel = _serviceProvider.GetService<TViewModel>();
        if (viewModel is null)
            return new Error($"ViewModel of type {typeof(TViewModel)} not found");

        _navigator.Push(viewModel);
        return Result.Ok(viewModel);
    }

    /// <summary>
    /// Navigates to a new ViewModel of type <typeparamref name="TViewModel"/> and clears the navigation stack.
    /// </summary>
    /// <remarks>
    /// This method resolves the ViewModel from the DI container, which avoids circular dependency issues
    /// in constructors.
    /// </remarks>
    /// <typeparam name="TViewModel">The type of the ViewModel to navigate to.</typeparam>
    /// <returns>A <see cref="Result{TViewModel}"/> containing the resolved ViewModel if successful; otherwise, an error.</returns>
    public Result<TViewModel> Goto<TViewModel>() where TViewModel : IViewModel
    {
        var viewModel = _serviceProvider.GetService<TViewModel>();
        if (viewModel is null)
            return new Error($"ViewModel of type {typeof(TViewModel)} not found");

        _navigator.Goto(viewModel);
        return Result.Ok(viewModel);
    }

    /// <summary>
    /// Replaces the current ViewModel with a new ViewModel of type <typeparamref name="TViewModel"/>.
    /// </summary>
    /// <remarks>
    /// This method resolves the ViewModel from the DI container, which avoids circular dependency issues
    /// in constructors.
    /// </remarks>
    /// <typeparam name="TViewModel">The type of the ViewModel to slide to.</typeparam>
    /// <returns>A <see cref="Result{TViewModel}"/> containing the resolved ViewModel if successful; otherwise, an error.</returns>
    public Result<TViewModel> Slide<TViewModel>() where TViewModel : IViewModel
    {
        var viewModel = _serviceProvider.GetService<TViewModel>();
        if (viewModel is null)
            return new Error($"ViewModel of type {typeof(TViewModel)} not found");

        _navigator.Slide(viewModel);
        return Result.Ok(viewModel);
    }
}