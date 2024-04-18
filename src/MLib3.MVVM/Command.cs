using System.ComponentModel;

namespace MLib3.MVVM
{
    
    // TODO: This Command and the IFluentReactionApi is shit - this would better be a fluent extension api and it should work with expressions (expression tree tutorial needed!!!)
    public abstract class Command : IFluentReactionApi
    {
        #region Fields
        private List<NotificationObserver> _notificationObservers = new List<NotificationObserver>();
        private Dictionary<INotifyPropertyChanged, List<List<string>>> _reactionPaths = new Dictionary<INotifyPropertyChanged, List<List<string>>>();
        private List<List<string>> _currentReactionPaths = null;
        private List<string> _currentReactionPath = null;
        #endregion

        #region Methods
        public ICommandReactionPath ReactsTo<TInstance>(TInstance target, string property = null) where TInstance : INotifyPropertyChanged
        {
            if (!_reactionPaths.ContainsKey(target))
                _reactionPaths.Add(target, new List<List<string>>());
            _currentReactionPaths = _reactionPaths[target];
            _currentReactionPath = new List<string>(new string[] { property });
            _currentReactionPaths.Add(_currentReactionPath);
            return this;
        }

        public ICommandReactionPath ThenToAll()
        {
            if (_currentReactionPath == null)
                throw new InvalidOperationException("First call ReactsTo!");
            _currentReactionPath.Add(null);
            return this;
        }

        public ICommandReactionSubPath ThenTo(string property)
        {
            if (_currentReactionPath == null)
                throw new InvalidOperationException("First call ReactsTo!");
            _currentReactionPath.Add(property);
            return this;
        }

        public ICommandReactionSubPath And(string property)
        {
            if (_currentReactionPath == null)
                throw new InvalidOperationException("First call ReactsTo!");
            _currentReactionPath = _currentReactionPath.ToList();
            _currentReactionPath[_currentReactionPath.Count - 1] = property;
            _currentReactionPaths.Add(_currentReactionPath);
            return this;
        }


        public void Activate()
        {
            foreach (INotifyPropertyChanged target in _reactionPaths.Keys)
            {
                if (target == null)
                    throw new ArgumentNullException("One of the target instances is null!");
                foreach (List<string> path in _reactionPaths[target])
                {
                    NotificationObserver observer = CreateObserverPath(path, RaiseCanExecuteChanged);
                    observer.SetObserved(target);
                    _notificationObservers.Add(observer);
                }
            }
        }

        public void Deactivate()
        {
            foreach (NotificationObserver observer in _notificationObservers)
            {
                DeleteObserverPath(observer);
            }
        }

        private NotificationObserver CreateObserverPath(List<string> path, Action callback)
        {
            if (path.Count <= 0)
                throw new ArgumentException("Path must not be empty!");
            NotificationObserver observer = new NotificationObserver(null, path.First(), callback);
            if (path.Count > 1)
                observer.SetChild(CreateObserverPath(path.Skip(1).ToList(), callback));
            return observer;
        }

        private void DeleteObserverPath(NotificationObserver observer)
        {
            NotificationObserver childObserver = observer.GetChild();
            if (childObserver != null)
                DeleteObserverPath(childObserver);
            observer.SetChild(null);
        }
        #endregion

        #region Events
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
