using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace MLib3.MVVM;

public static class CommandExtensions2
{
    // public static ReactionRoot<TCommand, TSource> React<TCommand, TSource>(this TCommand command, TSource source) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    // {
    //     return new ReactionRoot<TCommand, TSource>(command, source);
    // }
    //
    // public static PropertyChangedHandler<TCommand, TSource, TSource, TProperty> To<TCommand, TSource, TProperty>(this ReactionRoot<TCommand, TSource> root, Expression<Func<TSource, TProperty>> propertySelector) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    // {
    //     var handler = new PropertyChangedHandler<TCommand, TSource, TSource, TProperty>(root, propertySelector, () => root.Command.RaiseCanExecuteChanged());
    //     handler.Observe(root.Source);
    //     return handler;
    // }
    //
    // public static PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty> To<TCommand, TRootSource, TSource, TProperty, TNewProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler, Expression<Func<TRootSource, TNewProperty>> propertySelector) where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
    // {
    //     var newHandler = new PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty>(handler.Root, propertySelector, () => handler.Root.Command.RaiseCanExecuteChanged());
    //     newHandler.Observe(handler.Root.Source);
    //     return newHandler;
    // }
    
    public static PropertyChangedHandler<TCommand, TSource, TSource, TProperty> ReactTo<TCommand, TSource, TProperty>(this TCommand command, TSource source, Expression<Func<TSource, TProperty>> propertySelector) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        var handler = new PropertyChangedHandler<TCommand, TSource, TSource, TProperty>(new ReactionRoot<TCommand, TSource>(command, source), propertySelector);
        handler.Observe(source);
        return handler;
    }
    
    public static AnyPropertyChangedHandler<TCommand, TSource> ReactToAll<TCommand, TSource>(this TCommand command, TSource source) where TCommand : ICommandBase where TSource : INotifyPropertyChanged
    {
        var handler = new AnyPropertyChangedHandler<TCommand, TSource>(new ReactionRoot<TCommand, TSource>(command, source));
        handler.Observe(source);
        return handler;
    }
    
    public static PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty> ReactTo<TCommand, TRootSource, TSource, TProperty, TNewProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler, Expression<Func<TRootSource, TNewProperty>> propertySelector) where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
    {
        var newHandler = new PropertyChangedHandler<TCommand, TRootSource, TRootSource, TNewProperty>(handler.Root, propertySelector);
        newHandler.Observe(handler.Root.Source);
        return newHandler;
    }
    
    public static PropertyChangedHandler<TCommand, TRootSource, TRootSource, TProperty> ReactTo<TCommand, TRootSource, TProperty>(this AnyPropertyChangedHandler<TCommand, TRootSource> handler, Expression<Func<TRootSource, TProperty>> propertySelector) where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
    {
        var newHandler = new PropertyChangedHandler<TCommand, TRootSource, TRootSource, TProperty>(handler.Root, propertySelector);
        newHandler.Observe(handler.Root.Source);
        return newHandler;
    }
    
    public static AnyPropertyChangedHandler<TCommand, TRootSource> ReactToAll<TCommand, TRootSource, TSource, TProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler) where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
    {
        var newHandler = new AnyPropertyChangedHandler<TCommand, TRootSource>(handler);
        newHandler.Observe(handler.Root.Source);
        return newHandler;
    }
    
    public static PropertyChangedHandler<TCommand, TRootSource, TProperty, TSubProperty> ThenTo<TCommand, TRootSource, TSource, TProperty, TSubProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler, Expression<Func<TProperty, TSubProperty>> propertySelector) where TRootSource : INotifyPropertyChanged where TCommand : ICommandBase
    {
        return new PropertyChangedHandler<TCommand, TRootSource, TProperty, TSubProperty>(handler, propertySelector);
    }
    
    public static AnyPropertyChangedHandler<TCommand, TRootSource> ThenToAll<TCommand, TRootSource, TSource, TProperty>(this PropertyChangedHandler<TCommand, TRootSource, TSource, TProperty> handler) where TRootSource : INotifyPropertyChanged where TCommand : ICommandBase
    {
        return new AnyPropertyChangedHandler<TCommand, TRootSource>(handler);
    }
    
    public static IDisposable GetDisposable<TCommand, TRootSource>(this PropertyChangedHandler<TCommand, TRootSource> handler) where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
    {
        return handler.Root;
    }
}

public sealed class ReactionRoot<TCommand, TSource> : IDisposable where TCommand : ICommandBase where TSource : INotifyPropertyChanged
{
    private readonly List<PropertyChangedHandler<TCommand, TSource>> _handlers = new();
    
    public TSource Source { get; }
    public TCommand Command { get; }

    public ReactionRoot(TCommand command, TSource source)
    {
        Command = command;
        Source = source;
    }

    public void Add(PropertyChangedHandler<TCommand, TSource> handler)
    {
        _handlers.Add(handler);
    }

    public void Notify()
    {
        Command.RaiseCanExecuteChanged();
    }

    public void Dispose()
    {
        foreach (var handler in _handlers)
        {
            handler.Dispose();
        }
        _handlers.Clear();
    }
}

public interface IPropertyChangedHandler : IDisposable
{
    void Observe(INotifyPropertyChanged? source);
}

public abstract class PropertyChangedHandler<TCommand, TRootSource> : IPropertyChangedHandler where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
{
    protected ReactionRoot<TCommand, TRootSource> _root = null!;
    public ReactionRoot<TCommand, TRootSource> Root => _root;
    public abstract void Observe(INotifyPropertyChanged? source);
    public abstract void Dispose();
}

public abstract class SomePropertyChangedHandler<TCommand, TRootSource> : PropertyChangedHandler<TCommand, TRootSource> where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
{
    public abstract void AddChild(IPropertyChangedHandler child);
}

public sealed class AnyPropertyChangedHandler<TCommand, TRootSource> : PropertyChangedHandler<TCommand, TRootSource>, IPropertyChangedHandler where TCommand : ICommandBase where TRootSource : INotifyPropertyChanged
{
    private INotifyPropertyChanged? _source;

    public AnyPropertyChangedHandler(ReactionRoot<TCommand, TRootSource> root)
    {
        _root = root;
        _root.Add(this);
    }
    
    public AnyPropertyChangedHandler(SomePropertyChangedHandler<TCommand, TRootSource> parent)
    {
        _root = parent.Root;
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
        }
    }

    private void Handle(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == null) return;
        Root.Notify();
    }
    
    public override void Dispose()
    {
        if (_source is null) return;
        _source.PropertyChanged -= Handle;
        _source = null;
    }
}

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