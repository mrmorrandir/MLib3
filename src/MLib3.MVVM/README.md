# MLib3.MVVM

Concrete view model base classes for WPF / .NET MVVM applications. The implementations build on top of **CommunityToolkit.Mvvm** (`ObservableObject`, `ObservableValidator`) and the interfaces defined in `MLib3.MVVM.Abstractions`.

## Installation

```
dotnet add package MLib3.MVVM
```

## Classes

### `ViewModel`

The simplest base class. Inherits `ObservableObject` from CommunityToolkit.Mvvm and implements `IViewModel`. Use it for view models that only need property-change notifications.

Properties can be written manually with a backing field or — preferred — via the `[ObservableProperty]` source generator from CommunityToolkit.Mvvm.

```csharp
// Manual backing field
public class HomeViewModel : ViewModel
{
    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}

// Source-generator style
public partial class LoginViewModel : ViewModel
{
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;
}
```

### `ViewModel<TModel>`

Abstract, generic variant of `ViewModel`. The model is injected via the constructor and exposed through the `Model` property. Use it when a view model wraps a single domain object and needs to forward model properties to the UI.

```csharp
public class AuditEntryViewModel : ViewModel<AuditEntry>
{
    public AuditEntryViewModel(AuditEntry model) : base(model) { }

    public string Author => Model.Author;

    public DateTimeOffset Timestamp
    {
        get => Model.Timestamp;
        set => SetProperty(Model.Timestamp, value, Model, (m, v) => m.Timestamp = v);
    }
}
```

### `ValidatableViewModel`

Abstract base class for view models whose validation errors are **supplied from outside** — for example by a FluentValidation validator called from a command. Implements `IValidatableViewModel` and `INotifyDataErrorInfo`. Stores errors per property name and fires `ErrorsChanged` and `PropertyChanged` (for `HasErrors`) only when the error set actually changes.

Key members:

| Member                                   | Description                                                                           |
|------------------------------------------|---------------------------------------------------------------------------------------|
| `HasErrors`                              | `true` when at least one property has errors.                                         |
| `SetErrors(propertyName, params errors)` | Sets or replaces errors for a property. Pass no errors (or only whitespace) to clear. |
| `ClearErrors(propertyName)`              | Removes all errors for a specific property.                                           |
| `ClearErrors()`                          | Removes all errors across all properties.                                             |
| `GetErrors(propertyName)`                | Returns errors for a property, or all errors when `propertyName` is null/empty.       |

**Example with FluentValidation:**

```csharp
// The view model
public class PersonViewModel : ValidatableViewModel
{
    private string _name = string.Empty;
    private int _age;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public int Age
    {
        get => _age;
        set => SetProperty(ref _age, value);
    }
}

// The FluentValidation validator
public class PersonViewModelValidator : AbstractValidator<PersonViewModel>
{
    public PersonViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Age)
            .InclusiveBetween(0, 150).WithMessage("Age must be between 0 and 150.");
    }
}

// Applying validation results back to the view model
public class SavePersonCommand
{
    private readonly PersonViewModelValidator _validator = new();

    public void Execute(PersonViewModel vm)
    {
        vm.ClearErrors();

        var result = _validator.Validate(vm);

        foreach (var group in result.Errors.GroupBy(e => e.PropertyName))
            vm.SetErrors(group.Key, group.Select(e => e.ErrorMessage).ToArray());

        if (!vm.HasErrors)
        {
            // proceed with save
        }
    }
}
```

> **Tip:** Bind `HasErrors` in XAML to disable a save button:
> ```xml
> <Button Content="Save"
>         Command="{Binding SaveCommand}"
>         IsEnabled="{Binding HasErrors, Converter={StaticResource InverseBoolConverter}}" />
> ```

### `ValidatableViewModel<TModel>`

Generic, concrete subclass of `ValidatableViewModel`. Wraps a model of type `TModel` (passed via the constructor) and implements `IValidatableViewModel<TModel>`. The model is mutable via the `Model` property, which is useful when the view model needs to be recycled for different model instances.

```csharp
public class OrderViewModel : ValidatableViewModel<Order>
{
    public OrderViewModel(Order model) : base(model) { }

    public string Reference
        {
            get => Model.Reference;
            set => SetProperty(Model.Reference, value, Model, (m, v) => m.Reference = v);
        }
}
```

### `ViewModelValidator`

Abstract base class for view models that handle validation **internally**. Inherits `ObservableValidator` from CommunityToolkit.Mvvm and implements `IViewModelValidator`. Use it together with data annotation attributes and `ValidateAllProperties()` / `ValidateProperty()` from the toolkit.

```csharp
public class RegistrationViewModel : ViewModelValidator
{
    private string _email = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email
    {
        get => _email;
        set
        {
            SetProperty(ref _email, value, true); // true triggers validation
        }
    }
}
```

### `ViewModelValidator<TModel>`

Generic counterpart of `ViewModelValidator`. Wraps a `TModel` instance and implements `IViewModelValidator<TModel>`. The model is injected via the constructor.

```csharp
public class ProductViewModel : ViewModelValidator<Product>
{
    public ProductViewModel(Product model) : base(model) { }
}
```

### `ViewModelTracked<TModel>`

Extends `ValidatableViewModel<TModel>` with five observable state flags maintained via the CommunityToolkit.Mvvm `[ObservableProperty]` source generator:

| Property     | Description                                          |
|--------------|------------------------------------------------------|
| `HasChanges` | Set to `true` automatically by `SetTrackedProperty`. |
| `IsDeleted`  | Indicates the item has been logically deleted.       |
| `IsEditable` | Indicates the item can be edited.                    |
| `IsNew`      | Indicates the item has not yet been persisted.       |
| `IsSelected` | Indicates the item is selected in a list or grid.    |

Use the protected `SetTrackedProperty` method to update model-backed properties. It skips the update when the value has not changed, raises `PropertyChanging`/`PropertyChanged`, and automatically flips `HasChanges`.

```csharp
public class ArticleViewModel : ViewModelTracked<Article>
{
    public ArticleViewModel(Article model) : base(model) { }

    public string Title
    {
        get => Model.Title;
        set => SetTrackedProperty(Model.Title, value, v => Model.Title = v);
    }

    public decimal Price
    {
        get => Model.Price;
        set => SetTrackedProperty(Model.Price, value, v => Model.Price = v);
    }
}

// In a list view model:
var article = new ArticleViewModel(new Article { Title = "Widget", Price = 9.99m });
article.Title = "Super Widget"; // HasChanges becomes true automatically
```

### `ValueChangedCallback<T>`

A delegate type for callbacks that receive both the old and new value of a changed property.

```csharp
public delegate void ValueChangedCallback<in T>(T oldValue, T newValue);
```

Useful when custom logic needs to react to before/after values in property setters.




