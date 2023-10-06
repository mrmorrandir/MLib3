using System.Reflection;
using System.Runtime.CompilerServices;

namespace MLib3.MVVM;

public class ViewModel<TModel> : ViewModel, IViewModel<TModel> where TModel : class 
{
    public TModel Model { get; }

    public ViewModel(TModel model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }
    
    protected virtual void SetModel<TValue>(TValue? value, ValueChangedCallback<TValue?>? callback = null, [CallerMemberName] string property = null)
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

    public virtual object GetModel()
    {
        return Model;
    }
}