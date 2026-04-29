# MLib3.MVVM.Abstractions

Provides the core interfaces for the MLib3 MVVM infrastructure. This package is intended as a contracts-only reference — consume it in projects that need to depend on view model types without pulling in a concrete implementation.

## Installation

```
dotnet add package MLib3.MVVM.Abstractions
```

## Interfaces

### `IViewModel`

The base marker interface for all view models. Combines `INotifyPropertyChanged` and `INotifyPropertyChanging` so that data-binding frameworks can react to property changes in both directions.

```csharp
public interface IViewModel : INotifyPropertyChanged, INotifyPropertyChanging { }
```

### `IViewModel<TModel>`

Extends `IViewModel` with a strongly-typed `Model` property. Use this when a view model wraps a specific domain or data-transfer object and callers need to access the underlying model through the interface.

```csharp
public interface IViewModel<TModel> : IViewModel where TModel : class
{
    TModel Model { get; }
}
```

### `IValidatableViewModel`

Extends `IViewModel` and `INotifyDataErrorInfo` for scenarios where validation is **performed externally** — for example, by a dedicated validator service or a FluentValidation validator called from a command handler. Exposes `SetErrors`, `ClearErrors`, and `GetErrors` so that external components can push and remove errors without knowing the concrete view model type.

```csharp
public interface IValidatableViewModel : IViewModel, INotifyDataErrorInfo
{
    void SetErrors(string propertyName, params string[] errors);
    void ClearErrors(string propertyName);
    void ClearErrors();
    IEnumerable GetErrors(string? propertyName);
    bool HasErrors { get; }
    event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
```

### `IValidatableViewModel<TModel>`

Combines `IValidatableViewModel` and `IViewModel<TModel>` for typed, externally-validated view models.

### `IViewModelValidator`

Extends `IViewModel` and `INotifyDataErrorInfo` for scenarios where validation is **managed internally** by the view model itself — for example, by inheriting from `ObservableValidator` (CommunityToolkit.Mvvm). Unlike `IValidatableViewModel`, this interface does not expose public mutation methods for errors because the class manages its own error state.

### `IViewModelValidator<TModel>`

Combines `IViewModelValidator` and `IViewModel<TModel>` for strongly-typed, internally-validated view models.

### `IViewModelTracked`

Extends `IValidatableViewModel` with additional state-tracking properties useful in CRUD-heavy UIs such as data grids or master-detail forms.

| Property     | Description                                                      |
|--------------|------------------------------------------------------------------|
| `HasChanges` | `true` when the model has been modified since last load or save. |
| `IsDeleted`  | `true` when the item has been logically deleted.                 |
| `IsEditable` | `true` when the item is in an editable state.                    |
| `IsNew`      | `true` when the item has not yet been persisted.                 |
| `IsSelected` | `true` when the item is currently selected in the UI.            |

### `IViewModelTracked<TModel>`

Combines `IViewModelTracked` with the typed `TModel` constraint.

