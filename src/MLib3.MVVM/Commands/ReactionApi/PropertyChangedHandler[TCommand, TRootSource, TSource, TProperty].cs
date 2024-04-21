using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace MLib3.MVVM;

public sealed class PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> : SomePropertyChangedHandler<TCommand, TRootSource>, IPropertyChangedHandler where TRootSource : INotifyPropertyChanged where TCommand : ICommandBase
{
    
    private readonly string _propertyName;
    private readonly List<IPropertyChangedHandler> _children = new List<IPropertyChangedHandler>();
    private readonly Func<TSource,TProperty> _propertyGetter;
    private INotifyPropertyChanged? _source;
    
    public PropertyChangedHandler(ReactionRoot<TCommand, TRootSource> root, Expression<Func<TSource, TProperty>> propertySelector)
    {
        if (propertySelector.Body is not MemberExpression propertyMemberExpression)
            throw new ArgumentException("Property selector must be a member expression");
        _propertyName = propertyMemberExpression.Member.Name;
        _propertyGetter = propertySelector.Compile();
        _root = root;
        _root.Add(this);
    }

    public PropertyChangedHandler(SomePropertyChangedHandler<TCommand, TRootSource> parent, Expression<Func<TSource, TProperty>> propertySelector)
    {
        if (propertySelector.Body is not MemberExpression propertyMemberExpression)
            throw new ArgumentException("Property selector must be a member expression");
        _root = parent.Root;
        _propertyName = propertyMemberExpression.Member.Name;
        _propertyGetter = propertySelector.Compile();
        parent.AddChild(this);
    }
    
    public override void Observe(INotifyPropertyChanged? source)
    {
        if (_source is not null)
        {
            _source.PropertyChanged -= Handle;
        }
        _source = source;
        if (_source is not null)
        {
            _source.PropertyChanged += Handle;
            NotifyChildren();
        }
    }
    
    public override void AddChild(IPropertyChangedHandler child)
    {
        _children.Add(child);
        NotifyChild(child);
    }

    private void Handle(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == null || e.PropertyName != _propertyName) return;
        Root.Notify();
        NotifyChildren();
    }

    private void NotifyChildren()
    {
        if (!_children.Any()) return;
        
        var propertyValue = _source!.GetType().GetProperty(_propertyName)!.GetValue(_source);
        foreach (var child in _children)
            child.Observe(propertyValue as INotifyPropertyChanged);
    }
    private void NotifyChild(IPropertyChangedHandler child)
    {
        var propertyValue = _source!.GetType().GetProperty(_propertyName)!.GetValue(_source);
        child.Observe(propertyValue as INotifyPropertyChanged);
    }

    public override void Dispose()
    {
        if (_source is not null)
        {
            _source.PropertyChanged -= Handle;
            _source = null;
        }
        foreach (var child in _children)
        {
            child.Dispose();
        }
        _children.Clear();
    }
}