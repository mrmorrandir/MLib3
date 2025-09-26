using System.ComponentModel;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;

namespace MLib3.MVVM.Navigation;

public class Navigator<T> : INotifyPropertyChanged, INavigator<T> where T : IViewModel
{
    private readonly ILogger<Navigator<T>> _logger;
    private readonly Subject<NavigationChangedInfo<T>> _navigationChangedSubject = new();
    private readonly Subject<T> _reachedSubject = new();
    private readonly Subject<T> _leftSubject = new();
    private readonly Stack<T> _navigationStack = new();

    public IObservable<NavigationChangedInfo<T>> NavigationChanged => _navigationChangedSubject;
    public IObservable<T> Reached => _reachedSubject;
    public IObservable<T> Left => _leftSubject;
    

    public T CurrentViewModel => _navigationStack.Peek();

    public event PropertyChangedEventHandler? PropertyChanged;

    public Navigator(ILogger<Navigator<T>> logger)
    {
        _logger = logger;
        NavigationChanged.Subscribe(info =>
        {
            _reachedSubject.OnNext(info.Now);
            if (info.Before != null)
                _leftSubject.OnNext(info.Before);
        });
    }

    public void Push(T viewModel)
    {
        _navigationStack.TryPeek(out var beforeViewModel);
        _logger.LogDebug("Pushing {ViewModel} on top of {ViewModelBefore}", viewModel.GetType().Name, beforeViewModel?.GetType().Name ?? "nothing");
        _navigationStack.Push(viewModel);
        _navigationChangedSubject.OnNext(new NavigationChangedInfo<T>(beforeViewModel, viewModel));
        OnPropertyChanged(nameof(CurrentViewModel));
    }

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
    
    public void Slide(T viewModel)
    {
        _navigationStack.TryPop(out var beforeViewModel);
        _navigationStack.Push(viewModel);
        _logger.LogDebug("Sliding to {ViewModel} from {ViewModelBefore}", viewModel.GetType().Name, beforeViewModel?.GetType().Name ?? "nothing");
        _navigationChangedSubject.OnNext(new NavigationChangedInfo<T>(beforeViewModel, viewModel));
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public void Goto(T viewModel)
    {
        _navigationStack.TryPeek(out var beforeViewModel);
        _logger.LogDebug("Go to {ViewModel} from {ViewModelBefore} ", viewModel.GetType().Name, beforeViewModel?.GetType().Name ?? "nothing");
        _navigationStack.Clear();
        _navigationStack.Push(viewModel);
        _navigationChangedSubject.OnNext(new NavigationChangedInfo<T>(beforeViewModel, viewModel));
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}