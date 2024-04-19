using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace MLib3.MVVM;

public interface ICommandBase : ICommand
{
    void RaiseCanExecuteChanged();
}

public static class CommandExtensions
{
    public static IObservable<AnyPropertyChanged<TCommand, TSource>> React<TCommand, TSource>(this TCommand command, TSource source) where TCommand : ICommandBase where TSource : class, INotifyPropertyChanged
    {
        return new AnyPropertyChangedObserver<TCommand, TSource>(command, source);
    }
    
    public static IObservable<PropertyChanged<TCommand, TSource, TProperty>> To<TCommand, TSource, TProperty>(this IObservable<AnyPropertyChanged<TCommand, TSource>> source, Expression<Func<TSource, TProperty>> propertySelector) where TCommand : ICommandBase where TSource : class, INotifyPropertyChanged
    {
        return new PropertyChangedObserver<TCommand, TSource, TProperty>(source, propertySelector);
    }

    public static IObservable<SomePropertyChanged<TCommand, TSource>?> ToAll<TCommand, TSource>(this IObservable<AnyPropertyChanged<TCommand, TSource>> source) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        return new SomePropertyChangedObserver<TCommand, TSource>(source);
    }
    
    public static IDisposable Start<TCommand, TSource, TProperty>(this IObservable<PropertyChanged<TCommand, TSource, TProperty>> source) where TCommand : ICommandBase where TSource : INotifyPropertyChanged?
    {
        return source.Subscribe(x => x.TargetCommand.RaiseCanExecuteChanged());
    }
    
    public static IDisposable Start<TCommand, TSource>(this IObservable<SomePropertyChanged<TCommand, TSource>?> source) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        return source.Subscribe(x => x?.TargetCommand.RaiseCanExecuteChanged());
    }
    
    public static IObservable<PropertyChanged<TCommand, TProperty, TSubProperty>>  ThenTo<TCommand, TSource, TProperty, TSubProperty>(this IObservable<PropertyChanged<TCommand, TSource, TProperty>> source, Expression<Func<TProperty, TSubProperty>> subPropertySelector) where TCommand : ICommandBase where TSource : class, INotifyPropertyChanged where TProperty : INotifyPropertyChanged?
    {
        return new ThenToPropertyChangedObserver<TCommand, TSource, TProperty, TSubProperty>(source, subPropertySelector);
    }
}

public class AnyPropertyChanged<TCommand, TSource> where TCommand : ICommandBase
{
    public TCommand TargetCommand { get; }
    public TSource Source { get; }
    public string? PropertyName { get; }

    public AnyPropertyChanged(TCommand targetCommand, TSource source, string? propertyName)
    {
        TargetCommand = targetCommand;
        Source = source;
        PropertyName = propertyName;
    }
}

public class PropertyChanged<TCommand, TSource, TProperty> where TCommand : ICommandBase
{
    public TCommand TargetCommand { get; }
    public TSource Source { get; }
    public string? PropertyName { get; }
    public TProperty PropertyValue { get; }

    public PropertyChanged(TCommand targetCommand, TSource source, string? propertyName, TProperty propertyValue)
    {
        TargetCommand = targetCommand;
        Source = source;
        PropertyName = propertyName;
        PropertyValue = propertyValue;
    }
}

public class SomePropertyChanged<TCommand, TSource> where TCommand : ICommandBase
{
    public TCommand TargetCommand { get; }
    public TSource Source { get; }
    public string? PropertyName { get; }

    public SomePropertyChanged(TCommand targetCommand, TSource source, string? propertyName)
    {
        TargetCommand = targetCommand;
        Source = source;
        PropertyName = propertyName;
    }
}

public class AnyPropertyChangedObserver<TCommand, TSource> : 
    IObservable<AnyPropertyChanged<TCommand, TSource>>,
    IDisposable
    where TCommand : ICommandBase where TSource : INotifyPropertyChanged
{
    private readonly TCommand _targetCommand;
    private readonly TSource _source;
    private readonly Subject<AnyPropertyChanged<TCommand, TSource>> _subject = new();
    private readonly IDisposable _subscription;


    public AnyPropertyChangedObserver(TCommand targetCommand, TSource source)
    {
        _targetCommand = targetCommand;
        _source = source;
        _subscription = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
            h => source.PropertyChanged += h,
            h => source.PropertyChanged -= h).Subscribe(OnNext, OnError, OnCompleted);
    }
    
    private void OnCompleted()
    {
        _subject.OnCompleted();
    }

    private void OnError(Exception error)
    {
        _subject.OnError(error);
    }

    private void OnNext(EventPattern<PropertyChangedEventArgs> value)
    {
        _subject.OnNext(new AnyPropertyChanged<TCommand, TSource>(_targetCommand, _source, value.EventArgs.PropertyName));
    }

    public IDisposable Subscribe(IObserver<AnyPropertyChanged<TCommand, TSource>> observer)
    {
        return _subject.Subscribe(observer);
    }

    public void Dispose()
    {
        _subject.Dispose();
        _subscription.Dispose();
    }
}

public class PropertyChangedObserver<TCommand, TSource, TProperty> : 
    IObservable<PropertyChanged<TCommand, TSource, TProperty>>,
    IDisposable
    where TCommand : ICommandBase
{
    private readonly Subject<PropertyChanged<TCommand, TSource, TProperty>> _subject = new();
    private readonly Func<TSource,TProperty> _propertyGetter;
    private readonly string _propertyName;
    private readonly IDisposable _subscription;

    public PropertyChangedObserver(IObservable<AnyPropertyChanged<TCommand, TSource>> observable, Expression<Func<TSource, TProperty>> propertySelector)
    {
        _propertyName = ((MemberExpression)propertySelector.Body).Member.Name;
        _propertyGetter = propertySelector.Compile();
        _subscription = observable.Subscribe(OnNext, OnError, OnCompleted);
    }
    
    private void OnCompleted()
    {
        _subject.OnCompleted();
    }

    private void OnError(Exception error)
    {
        _subject.OnError(error);
    }

    private void OnNext(AnyPropertyChanged<TCommand, TSource> value)
    {
        if (value.PropertyName != _propertyName) return;
        
        var propertyValue = _propertyGetter(value.Source);
        if (!_subject.HasObservers || propertyValue is null) 
            value.TargetCommand.RaiseCanExecuteChanged();
        
        _subject.OnNext(new PropertyChanged<TCommand, TSource, TProperty>(value.TargetCommand, value.Source, value.PropertyName, propertyValue));
    }

    public IDisposable Subscribe(IObserver<PropertyChanged<TCommand, TSource, TProperty>> observer)
    {
        return _subject.Subscribe(observer);
    }

    public void Dispose()
    {
        _subject.Dispose();
        _subscription.Dispose();
    }
}

public class ThenToPropertyChangedObserver<TCommand, TSource, TProperty, TSubProperty> : 
    IObservable<PropertyChanged<TCommand, TProperty, TSubProperty>>,
    IDisposable
    where TCommand : ICommandBase
{
    private readonly Subject<PropertyChanged<TCommand, TProperty, TSubProperty>> _subject = new();
    private readonly Func<TProperty, TSubProperty> _subPropertyGetter;
    private readonly string _subPropertyName;
    private readonly IDisposable _subscription;

    public ThenToPropertyChangedObserver(IObservable<PropertyChanged<TCommand, TSource, TProperty>> observable, Expression<Func<TProperty, TSubProperty>> propertySelector)
    {
        _subPropertyName = ((MemberExpression)propertySelector.Body).Member.Name;
        _subPropertyGetter = propertySelector.Compile();
        _subscription = observable.Subscribe(OnNext, OnError, OnCompleted);
    }
    
    private void OnCompleted()
    {
        _subject.OnCompleted();
    }

    private void OnError(Exception error)
    {
        _subject.OnError(error);
    }

    private void OnNext(PropertyChanged<TCommand, TSource, TProperty> value)
    {
        if (value.PropertyName != _subPropertyName) return;
        if (value.PropertyValue is null)
        {
            value.TargetCommand.RaiseCanExecuteChanged();
            return;
        }
        
        var subPropertyValue = _subPropertyGetter(value.PropertyValue);
        if (!_subject.HasObservers || subPropertyValue is null)
        {
            value.TargetCommand.RaiseCanExecuteChanged();
            return;
        }

        _subject.OnNext(new PropertyChanged<TCommand, TProperty, TSubProperty>(value.TargetCommand, value.PropertyValue, _subPropertyName, subPropertyValue));
    }
    
    public IDisposable Subscribe(IObserver<PropertyChanged<TCommand, TProperty, TSubProperty>> observer)
    {
        return _subject.Subscribe(observer);
    }

    public void Dispose()
    {
        _subject.Dispose();
        _subscription.Dispose();
    }
}


public class SomePropertyChangedObserver<TCommand, TSource> : 
    IObservable<SomePropertyChanged<TCommand, TSource>>,
    IDisposable
    where TCommand : ICommandBase
{
    private readonly Subject<SomePropertyChanged<TCommand, TSource>> _subject = new();
    private readonly IDisposable _subscription;

    public SomePropertyChangedObserver(IObservable<AnyPropertyChanged<TCommand, TSource>> observable)
    {
        _subscription = observable.Subscribe(OnNext, OnError, OnCompleted);
    }

    private void OnCompleted()
    {
        _subject.OnCompleted();
    }

    private void OnError(Exception error)
    {
        _subject.OnError(error);
    }

    private void OnNext(AnyPropertyChanged<TCommand, TSource> value)
    {
        if (!_subject.HasObservers) 
            value.TargetCommand.RaiseCanExecuteChanged();
        
        _subject.OnNext(new SomePropertyChanged<TCommand, TSource>(value.TargetCommand, value.Source, value.PropertyName));
    }

    public IDisposable Subscribe(IObserver<SomePropertyChanged<TCommand, TSource>> observer)
    {
        return _subject.Subscribe(observer);
    }
    
    public void Dispose()
    {
        _subject.Dispose();
        _subscription.Dispose();
    }

}