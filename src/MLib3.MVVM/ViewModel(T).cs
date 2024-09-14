using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MLib3.MVVM;

public abstract class ViewModel<TModel> : ViewModel, IViewModel<TModel> where TModel : class 
{
    public TModel Model { get; }

    public ViewModel(TModel model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }
    
    protected virtual void SetModel<TValue>(TValue? value, ValueChangedCallback<TValue?>? callback = null, [CallerMemberName] string? property = null)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));
        var propertyInfo = Model.GetType().GetProperty(property);
        if (propertyInfo is null) throw new ArgumentException($"Property {property} not found on model {Model.GetType().Name}");
        var oldValue = (TValue?)propertyInfo.GetValue(Model, null);
        if (EqualityComparer<TValue>.Default.Equals(oldValue, value)) return;
        propertyInfo.SetValue(Model, value, null);
        callback?.Invoke(oldValue, value);
        OnPropertyChanged(property);
    }
    
    private readonly ConcurrentDictionary<string, Delegate> _propertySetters = new();
    private readonly ConcurrentDictionary<string, Delegate> _propertyGetters = new();
    protected virtual void SetModel2<TValue>(TValue? value, ValueChangedCallback<TValue?>? callback = null, [CallerMemberName] string? property = null)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));

        var setter = (Action<TModel, TValue?>)_propertySetters.GetOrAdd(property, prop =>
        {
            var parameterExpression = Expression.Parameter(typeof(TModel), "model");
            var valueExpression = Expression.Parameter(typeof(TValue), "value");
            var propertyInfo = typeof(TModel).GetProperty(prop) ?? throw new ArgumentException($"Property {prop} not found on model {typeof(TModel).Name}");
            var propertySetter = propertyInfo.GetSetMethod(true) ?? throw new ArgumentException($"Property {prop} does not have a setter");
            var setterCall = Expression.Call(parameterExpression, propertySetter, valueExpression);
            return Expression.Lambda<Action<TModel, TValue?>>(setterCall, parameterExpression, valueExpression).Compile();
        });

        var getter = (Func<TModel, TValue?>)_propertyGetters.GetOrAdd(property, prop =>
        {
            var parameterExpression = Expression.Parameter(typeof(TModel), "model");
            var propertyInfo = typeof(TModel).GetProperty(prop) ?? throw new ArgumentException($"Property {prop} not found on model {typeof(TModel).Name}");
            var propertyGetter = propertyInfo.GetGetMethod(true) ?? throw new ArgumentException($"Property {prop} does not have a getter");
            var getterCall = Expression.Call(parameterExpression, propertyGetter);
            return Expression.Lambda<Func<TModel, TValue?>>(getterCall, parameterExpression).Compile();
        });

        var oldValue = getter(Model);
        if (EqualityComparer<TValue>.Default.Equals(oldValue, value)) return;

        setter(Model, value);
        callback?.Invoke(oldValue, value);
        OnPropertyChanged(property);
    }
    
    private readonly ConcurrentDictionary<string, (Delegate Getter, Delegate Setter)> _propertyAccessors = new();
    protected virtual void SetModel3<TValue>(TValue? value, ValueChangedCallback<TValue?>? callback = null, [CallerMemberName] string? property = null)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));

        var accessors = _propertyAccessors.GetOrAdd(property, prop =>
        {
            var parameterExpression = Expression.Parameter(typeof(TModel), "model");
            var valueExpression = Expression.Parameter(typeof(TValue), "value");
            var propertyInfo = typeof(TModel).GetProperty(prop) ?? throw new ArgumentException($"Property {prop} not found on model {typeof(TModel).Name}");

            var propertySetter = propertyInfo.GetSetMethod(true) ?? throw new ArgumentException($"Property {prop} does not have a setter");
            var setterCall = Expression.Call(parameterExpression, propertySetter, valueExpression);
            var setter = Expression.Lambda<Action<TModel, TValue?>>(setterCall, parameterExpression, valueExpression).Compile();

            var propertyGetter = propertyInfo.GetGetMethod(true) ?? throw new ArgumentException($"Property {prop} does not have a getter");
            var getterCall = Expression.Call(parameterExpression, propertyGetter);
            var getter = Expression.Lambda<Func<TModel, TValue?>>(getterCall, parameterExpression).Compile();

            return (Getter: getter, Setter: setter);
        });

        var getter = (Func<TModel, TValue?>)accessors.Getter;
        var setter = (Action<TModel, TValue?>)accessors.Setter;

        var oldValue = getter(Model);
        if (EqualityComparer<TValue>.Default.Equals(oldValue, value)) return;

        setter(Model, value);
        callback?.Invoke(oldValue, value);
        OnPropertyChanged(property);
    }
}