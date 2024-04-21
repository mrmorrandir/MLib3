using System.ComponentModel;
using System.Linq.Expressions;

namespace MLib3.MVVM;

public static class CommandExtensions
{
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