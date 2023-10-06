namespace MLib3.MVVM;

internal class Unsubscriber<T> : IDisposable
{
    private readonly ICollection<IObserver<T>> _observers;
    private readonly IObserver<T> _observer;

    public Unsubscriber(ICollection<IObserver<T>> observers, IObserver<T> observer)
    {
        _observers = observers ?? throw new ArgumentNullException(nameof(observers));
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));
    }
    public void Dispose()
    {
        if (_observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}