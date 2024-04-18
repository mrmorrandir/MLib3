using System.ComponentModel;

namespace MLib3.MVVM
{
    internal class NotificationObserver
    {
        private INotifyPropertyChanged _observed = null;
        private NotificationObserver _childObserver = null;
        private string _property = null;
        private Action _callback = null;

        public NotificationObserver(INotifyPropertyChanged observed, string property, Action callback)
        {
            _observed = observed;
            _property = property;
            _callback = callback;
        }

        public NotificationObserver SetChild(NotificationObserver childObserver)
        {
            _childObserver = childObserver;
            return childObserver;
        }

        public NotificationObserver GetChild()
        {
            return _childObserver;
        }

        public void SetObserved(INotifyPropertyChanged newObserved)
        {
            try
            {
                if (_observed != null)
                    _observed.PropertyChanged -= OnPropertyChanged;
            }
            catch { }
            _observed = newObserved;
            try
            {
                if (_observed != null)
                    _observed.PropertyChanged += OnPropertyChanged;
            }
            catch { }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                if (_property != null)
                {
                    if (e.PropertyName == _property)
                    {
                        if (_childObserver != null)
                        {
                            UnChain(_childObserver);
                            INotifyPropertyChanged newObserved = sender.GetType().GetProperty(e.PropertyName).GetValue(sender) as INotifyPropertyChanged;
                            _childObserver.SetObserved(newObserved);
                        }
                        _callback();
                    } // else no callback
                } else
                {
                    _callback();
                }
            }
            else
            {
                _callback();
            }
        }

        private void UnChain(NotificationObserver observer)
        {
            if (observer._childObserver != null)
                UnChain(observer._childObserver);
            try
            {
                if (observer._observed != null)
                    observer._observed.PropertyChanged -= observer.OnPropertyChanged;
            }
            catch { }
        }

    }
}
