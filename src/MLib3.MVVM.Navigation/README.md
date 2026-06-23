# MLib3.MVVM.Navigation

Stack-based view model navigation for WPF / .NET MVVM applications. The navigator manages a history stack and exposes reactive observables for integration with `System.Reactive` or any Rx-compatible framework.

## Installation

```
dotnet add package MLib3.MVVM.Navigation
```

## Dependency Injection

Register a navigator with the provided extension methods. There are two ways to register and use the navigator:

### Generic Navigator (`INavigator<T>`)

Use this when you want to manage a specific base type of ViewModels and you prefer to provide ViewModel instances manually.

```csharp
builder.Services.AddNavigation<IPageViewModel>();
```

This registers `Navigator<IPageViewModel>` as a singleton for `INavigator<IPageViewModel>`.

### Non-Generic Navigator (`INavigator`)

Use this when you want to leverage Dependency Injection to resolve ViewModels automatically. This is especially useful for **resolving circular dependencies** (e.g., ViewModel A navigates to ViewModel B, and ViewModel B needs to navigate back to ViewModel A).

```csharp
builder.Services.AddNavigation();
```

This registers the non-generic `INavigator` (which internally manages `IViewModel`).

---

## Classes and Interfaces

### `INavigator<T>`

The contract for the navigation service where you provide instances manually. `T` must implement `IViewModel`.

| Member              | Description                                                                                             |
|---------------------|---------------------------------------------------------------------------------------------------------|
| `CurrentViewModel`  | The view model currently on top of the navigation stack.                                                |
| `NavigationChanged` | Observable that emits a `NavigationChangedInfo<T>` on every navigation action.                          |
| `Reached`           | Observable that emits the view model that was navigated **to**.                                         |
| `Left`              | Observable that emits the view model that was navigated **away from**.                                  |
| `Push(viewModel)`   | Pushes a new view model onto the stack.                                                                 |
| `Pop()`             | Removes the top view model and returns to the previous one. Ignored when only one item is on the stack. |
| `Slide(viewModel)`  | Replaces the top of the stack without keeping the previous entry in history.                            |
| `Goto(viewModel)`   | Clears the entire stack and pushes the given view model as the only entry.                              |

### `INavigator`

Extends `INavigator<IViewModel>` and adds methods to resolve ViewModels via DI.

| Member                | Description                                                                    |
|-----------------------|--------------------------------------------------------------------------------|
| `Push<TViewModel>()`  | Resolves `TViewModel` from DI and pushes it onto the stack.                    |
| `Slide<TViewModel>()` | Resolves `TViewModel` from DI and replaces the top of the stack.               |
| `Goto<TViewModel>()`  | Resolves `TViewModel` from DI, clears the stack, and pushes the new ViewModel. |

### `Navigator<T>` and `Navigator`

Concrete implementations. They use `System.Reactive` subjects internally and log navigation transitions at `Debug` level.

Both classes are implemented as **`partial` classes**, which allows you to extend them with domain-specific navigation methods.

**Navigation actions:**

- **Push** — navigate forward while preserving history (e.g. opening a detail page).
- **Pop** — navigate back (e.g. closing a detail page).
- **Slide** — replace the current page without adding to history (e.g. switching between sibling pages in a wizard).
- **Goto** — jump to a root page and discard all history (e.g. logging out and returning to the login screen).

## Examples

### Using `INavigator<T>` (Manual instantiation/injection)

```csharp
public interface IPageViewModel : IViewModel { }
public class HomeViewModel : ViewModel, IPageViewModel { }
public class DetailViewModel : ViewModel, IPageViewModel { }

// Registration
builder.Services.AddNavigation<IPageViewModel>();
builder.Services.AddTransient<HomeViewModel>();
builder.Services.AddTransient<DetailViewModel>();

// Usage
public class ShellViewModel(INavigator<IPageViewModel> navigator, HomeViewModel home) : ViewModel
{
    public void Initialize() => navigator.Push(home);
}
```

### Using `INavigator` (DI-based, resolves circular dependencies)

In this example, `A` can navigate to `B` and `B` can navigate to `A` without constructor injection loops, because the navigator resolves the target ViewModel only when needed.

```csharp
public class ViewModelA(INavigator navigator) : ViewModel, IViewModel
{
    public void GoToB() => navigator.Push<ViewModelB>();
}

public class ViewModelB(INavigator navigator) : ViewModel, IViewModel
{
    public void GoToA() => navigator.Push<ViewModelA>();
}

// Registration
builder.Services.AddNavigation(); // Registers INavigator
builder.Services.AddTransient<ViewModelA>();
builder.Services.AddTransient<ViewModelB>();
```

### Custom Navigation Methods (Extending via `partial`)

Because the `Navigator` classes are `partial`, you can add your own navigation methods to make your code more expressive. 

> [!IMPORTANT]
> Custom navigation methods should only focus on the **navigation act itself**. Business logic, like loading data or initializing properties, should remain within the ViewModels or handled via navigation observables.

```csharp
// In your project
namespace MLib3.MVVM.Navigation;

public partial class Navigator
{
    // A clean way to navigate without type parameters in the ViewModel
    public void GotoSettings() => Goto<SettingsViewModel>();
    
    // Pass existing instances or just use internal Push
    public void PushProductDetails(ProductDetailsViewModel vm) => Push(vm);
}
```

Usage in a ViewModel:

```csharp
public class MainViewModel(INavigator navigator) : ViewModel
{
    // Use the concrete type or cast to use partial methods
    private readonly Navigator _navigator = (Navigator)navigator;

    public void OpenSettings() => _navigator.GotoSettings();
}
```

### XAML Integration

Bind `Navigator.CurrentViewModel` in XAML with a `DataTemplate` selector or a view locator to display the correct view automatically:

```xml
<ContentControl Content="{Binding Navigator.CurrentViewModel}" />
```

## `NavigationChangedInfo<T>`

Carries information about a navigation transition.

| Property | Description                                                                     |
|----------|---------------------------------------------------------------------------------|
| `Before` | The view model that was active before the transition. `null` on the first push. |
| `Now`    | The view model that is active after the transition.                             |

The class also provides an implicit conversion to `T` (returning `Now`) and equality operators against `T` for convenient stream filtering.

```csharp
// React to navigation events
_navigator.NavigationChanged
    .Where(info => info == myDetailViewModel)
    .Subscribe(_ => LoadDetailData());

// React to leaving a page
_navigator.Left
    .OfType<DetailViewModel>()
    .Subscribe(vm => vm.ClearErrors());
```

