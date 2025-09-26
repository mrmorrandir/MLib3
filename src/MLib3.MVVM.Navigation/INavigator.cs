using System.ComponentModel;

namespace MLib3.MVVM.Navigation;

public interface INavigator<T> where T : IViewModel
{
    IObservable<NavigationChangedInfo<T>> NavigationChanged { get; }
    T CurrentViewModel { get; }
    IObservable<T> Reached { get; }
    IObservable<T> Left { get; }
    event PropertyChangedEventHandler PropertyChanged;
    void Push(T viewModel);
    void Pop();
    void Goto(T viewModel);
    void Slide(T viewModel);
}