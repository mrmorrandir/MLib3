using System.ComponentModel;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;

namespace MLib3.MVVM.Navigation;

/// <summary>
/// Provides navigation functionalities for view models of type <typeparamref name="T"/>.
/// </summary>
/// <remarks>
/// <para>
/// This implementation of <see cref="INavigator{T}"/> manages a stack of ViewModels.
/// It requires ViewModels to be passed explicitly to its navigation methods.
/// </para>
/// <para>
/// The class is marked as <c>partial</c> to allow developers to extend it with domain-specific
/// navigation methods. These extensions should focus solely on navigation transitions.
/// </para>
/// </remarks>
/// <typeparam name="T">The base type of view models managed by this navigator.</typeparam>
public partial class Navigator<T> : INotifyPropertyChanged, INavigator<T> where T : IViewModel
{
    private readonly ILogger<Navigator<T>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Subject<NavigationChangedInfo<T>> _navigationChangedSubject = new();
    private readonly Subject<T> _reachedSubject = new();
    private readonly Subject<T> _leftSubject = new();
    private readonly Stack<T> _navigationStack = new();

    /// <inheritdoc />
    public IObservable<NavigationChangedInfo<T>> NavigationChanged => _navigationChangedSubject;
    /// <inheritdoc />
    public IObservable<T> Reached => _reachedSubject;
    /// <inheritdoc />
    public IObservable<T> Left => _leftSubject;
    

    /// <inheritdoc />
    public T CurrentViewModel => _navigationStack.Peek();

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="Navigator{T}"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="serviceProvider">The service provider instance.</param>
    public Navigator(ILogger<Navigator<T>> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        NavigationChanged.Subscribe(info =>
        {
            _reachedSubject.OnNext(info.Now);
            if (info.Before != null)
                _leftSubject.OnNext(info.Before);
        });
    }

    /// <inheritdoc />
    public void Push(T viewModel)
    {
        _navigationStack.TryPeek(out var beforeViewModel);
        _logger.LogDebug("Pushing {ViewModel} on top of {ViewModelBefore}", viewModel.GetType().Name, beforeViewModel?.GetType().Name ?? "nothing");
        _navigationStack.Push(viewModel);
        _navigationChangedSubject.OnNext(new NavigationChangedInfo<T>(beforeViewModel, viewModel));
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    /// <inheritdoc />
    public void Pop()
    {
        if (_navigationStack.Count > 1)
        {
            _navigationStack.TryPop(out var beforeViewModel);
            var currentViewModel = _navigationStack.Peek();
            _logger.LogDebug("Popping {ViewModel} from {ViewModelBefore}", currentViewModel.GetType().Name, beforeViewModel?.GetType().Name ?? "nothing");
            _navigationChangedSubject.OnNext(new NavigationChangedInfo<T>(beforeViewModel, currentViewModel));
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
    
    /// <inheritdoc />
    public void Slide(T viewModel)
    {
        _navigationStack.TryPop(out var beforeViewModel);
        _navigationStack.Push(viewModel);
        _logger.LogDebug("Sliding to {ViewModel} from {ViewModelBefore}", viewModel.GetType().Name, beforeViewModel?.GetType().Name ?? "nothing");
        _navigationChangedSubject.OnNext(new NavigationChangedInfo<T>(beforeViewModel, viewModel));
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    /// <inheritdoc />
    public void Goto(T viewModel)
    {
        _navigationStack.TryPeek(out var beforeViewModel);
        _logger.LogDebug("Go to {ViewModel} from {ViewModelBefore} ", viewModel.GetType().Name, beforeViewModel?.GetType().Name ?? "nothing");
        _navigationStack.Clear();
        _navigationStack.Push(viewModel);
        _navigationChangedSubject.OnNext(new NavigationChangedInfo<T>(beforeViewModel, viewModel));
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}