using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace MLib3.MVVM;

public static class CommandExtensions2
{
    public static ReactionRoot<TCommand, TSource> ReactTo2<TCommand, TSource>(this TCommand command, TSource source) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        return new ReactionRoot<TCommand, TSource>(command, source);
    }

    public static PropertyChangedHandler<TCommand, TSource, TSource, TProperty> To<TCommand, TSource, TProperty>(this ReactionRoot<TCommand, TSource> root, Expression<Func<TSource, TProperty>> propertySelector) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        var handler = new PropertyChangedHandler<TCommand, TSource, TSource, TProperty>(root, propertySelector, () => root.Command.RaiseCanExecuteChanged());
        handler.Observe(root.Source);
        return handler;
    }
    
    public static PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty> To<TCommand, TRootSource, TSource, TProperty, TNewProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler, Expression<Func<TRootSource, TNewProperty>> propertySelector) where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
    {
        var newHandler = new PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty>(handler.Root, propertySelector, () => handler.Root.Command.RaiseCanExecuteChanged());
        newHandler.Observe(handler.Root.Source);
        return newHandler;
    }
    
    public static PropertyChangedHandler<TCommand, TSource, TSource, TProperty> ReactTo<TCommand, TSource, TProperty>(this TCommand command, TSource source, Expression<Func<TSource, TProperty>> propertySelector) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        var handler = new PropertyChangedHandler<TCommand, TSource, TSource, TProperty>(new ReactionRoot<TCommand, TSource>(command, source), propertySelector, () => command.RaiseCanExecuteChanged());
        handler.Observe(source);
        return handler;
    }
    
    public static PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty> ReactTo<TCommand, TRootSource, TSource, TProperty, TNewProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler, Expression<Func<TRootSource, TNewProperty>> propertySelector) where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
    {
        var newHandler = new PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty>(handler.Root, propertySelector, () => handler.Root.Command.RaiseCanExecuteChanged());
        newHandler.Observe(handler.Root.Source);
        return newHandler;
    }
    
    public static PropertyChangedHandler<TCommand, TRootSource, TProperty, TSubProperty> ThenTo<TCommand, TRootSource, TSource, TProperty, TSubProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler, Expression<Func<TProperty, TSubProperty>> propertySelector) where TRootSource : INotifyPropertyChanged where TCommand : ICommandBase
    {
        var child = new PropertyChangedHandler<TCommand, TRootSource, TProperty, TSubProperty>(handler, propertySelector);
        return child;
    }
    
}

public class ReactionRoot<TCommand, TSource> : IDisposable where TCommand : ICommandBase where TSource : INotifyPropertyChanged
{
    private readonly TCommand _command;
    private readonly TSource _source;
    private readonly List<PropertyChangedHandler<TCommand, TSource>> _handlers = new();
    
    public TSource Source => _source;
    public TCommand Command => _command;
    
    public ReactionRoot(TCommand command, TSource source)
    {
        _command = command;
        _source = source;
    }

    public void Add(PropertyChangedHandler<TCommand, TSource> handler)
    {
        _handlers.Add(handler);
    }

    public void Dispose()
    {
        foreach (var handler in _handlers)
        {
            handler.Dispose();
        }
    }
}



public interface IPropertyChangedHandler : IDisposable
{
    void Observe(INotifyPropertyChanged? source);
    void AddChild(IPropertyChangedHandler child);
}

public abstract class PropertyChangedHandler<TCommand, TRootSource> : IPropertyChangedHandler, IDisposable where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
{
    protected internal Action? _changedCallback;
    protected internal ReactionRoot<TCommand, TRootSource> _root;
    public ReactionRoot<TCommand, TRootSource> Root => _root;
    public abstract void Observe(INotifyPropertyChanged? source);
    public abstract void AddChild(IPropertyChangedHandler child);
    public abstract void Dispose();
}

public class PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> : PropertyChangedHandler<TCommand, TRootSource>, IPropertyChangedHandler, IDisposable where TRootSource : INotifyPropertyChanged where TCommand : ICommandBase
{
    
    private readonly string _propertyName;
    private readonly List<IPropertyChangedHandler> _children = new List<IPropertyChangedHandler>();
    private readonly Func<TSource,TProperty> _propertyGetter;
    private INotifyPropertyChanged? _source;
    
    public PropertyChangedHandler(ReactionRoot<TCommand, TRootSource> root, Expression<Func<TSource, TProperty>> propertySelector, Action changedCallback)
    {
        if (propertySelector.Body is not MemberExpression propertyMemberExpression)
            throw new ArgumentException("Property selector must be a member expression");
        _root = root;
        _propertyName = propertyMemberExpression.Member.Name;
        _propertyGetter = propertySelector.Compile();
        _changedCallback = changedCallback;
    }

    public PropertyChangedHandler(PropertyChangedHandler<TCommand, TRootSource> parent, Expression<Func<TSource, TProperty>> propertySelector)
    {
        if (propertySelector.Body is not MemberExpression propertyMemberExpression)
            throw new ArgumentException("Property selector must be a member expression");
        _root = parent._root;
        _propertyName = propertyMemberExpression.Member.Name;
        _propertyGetter = propertySelector.Compile();
        _changedCallback = parent._changedCallback;
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
        _changedCallback?.Invoke();
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
        }
        foreach (var child in _children)
        {
            child.Dispose();
        }
    }
}

public class TestVM : ViewModel
{
    private A _a = new A();
    private B _b = new B();
    private DelegateCommand _command = new DelegateCommand(_ => { }, _ => true);

    public A A
    {
        get => _a;
        set => Set(ref _a, value);
    }

    public B B
    {
        get => _b;
        set => Set(ref _b, value);
    }
    
    public TestVM()
    {
        
    }
}

public class A : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private B _b = new B();
    public B B
    {
        get => _b;
        set
        {
            _b = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(B)));
        }
    }
}

public class B : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private C _c = new C();
    public C C
    {
        get => _c;
        set
        {
            _c = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(C)));
        }
    }
}

public class C : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string _name = nameof(C);
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }
}