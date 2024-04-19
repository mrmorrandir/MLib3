using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace MLib3.MVVM;

public interface ICommandBase : ICommand
{
    void RaiseCanExecuteChanged();
}

public static class CommandExtensions
{
    public static ReactionSource<TCommand, TSource> ReactTo<TCommand, TSource>(this TCommand command, TSource source) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        return new ReactionSource<TCommand, TSource>(command, source);
    }
}

public class ReactionSource<TCommand, TSource> where TCommand : ICommandBase where TSource : INotifyPropertyChanged
{
    private readonly TCommand _command;
    private readonly IObservable<EventPattern<PropertyChangedEventArgs>> _sourceObservable;


    public ReactionSource(TCommand command, TSource source)
    {
        _command = command;
        _sourceObservable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
            h => source.PropertyChanged += h,
            h => source.PropertyChanged -= h);
    }

    public IObservable<TProperty> Property<TProperty>(Expression<Func<TSource, TProperty>> property)
    {
        var propertyName = ((MemberExpression)property.Body).Member.Name;
        var propertyGetter = property.Compile();
        var propertyObservable = _sourceObservable.Where(e => e.EventArgs.PropertyName == propertyName);
        propertyObservable.Subscribe(_ => RaiseCanExecuteChanged());
        return propertyObservable.Select(x => propertyGetter((TSource)x.Sender));
    }

    public IObservable<string?> AllProperties()
    {
        _sourceObservable.Subscribe(_ => RaiseCanExecuteChanged());
        return _sourceObservable.Select(x => x.EventArgs.PropertyName);
    }

    private void RaiseCanExecuteChanged()
    {
        _command.RaiseCanExecuteChanged();
    }
}

// public class PropertySource<TProperty, TCommand> : IDisposable where TProperty : INotifyPropertyChanged where TCommand : ICommandBase
// {
//     private readonly TCommand _command;
//     private readonly IObservable<TProperty> _observable;
//     private readonly IDisposable _subscription;
//     private readonly TProperty? _propertyValueBefore = default;
//
//     public PropertySource(IObservable<TProperty> observable, TCommand command)
//     {
//         _observable = observable;
//         _command = command;
//         _subscription = _observable.Subscribe(_ =>
//         {
//             _command.RaiseCanExecuteChanged();
//         });
//     }
//
//     public void Dispose()
//     {
//         _subscription.Dispose();
//     }
//
//     public void Then<TSubProperty>(Expression<Func<TProperty, TSubProperty>> subProperty)
//     {
//         var subPropertyName = ((MemberExpression)subProperty.Body).Member.Name;
//         var subPropertyGetter = subProperty.Compile();
//         
//         // x.Property.Property
//         // x.Property = new Class{ Property = new Property() }
//         // x.Property changes => I want to get PropertyChanged of the new Class.Property
//         var subPropertySubscription = _observable.Subscribe(value =>
//         {
//             if (_propertyValueBefore != null)
//             {
//                 
//             }   
//             var subPropertyObservable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
//                 h => value.PropertyChanged += h,
//                 h => value.PropertyChanged -= h);
//         })
//
//         // When TProperty is of type INotifyPropertyChanged we can subscribe to its PropertyChanged event
//         var source = (INotifyPropertyChanged)_observable;
//         var _sourceObservable =
//             Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
//                 h => source.PropertyChanged += h,
//                 h => source.PropertyChanged -= h);
//
//         _observable.Where(x => (INotifyPropertyChanged)x..Select(subPropertyGetter).Subscribe(_ => _command.RaiseCanExecuteChanged());
//     }
// }