# MLib3.MVVM.Navigation

Stack-based view model navigation for WPF / .NET MVVM applications. The navigator manages a history stack and exposes reactive observables for integration with `System.Reactive` or any Rx-compatible framework.

## Installation

```
dotnet add package MLib3.MVVM.Navigation
```

## Dependency Injection

Register a navigator for a specific view model base type with the provided extension method:

```csharp
builder.Services.AddNavigation<IPageViewModel>();
```

This registers `Navigator<IPageViewModel>` as a singleton for `INavigator<IPageViewModel>`.

## Classes and Interfaces

### `INavigator<T>`

The contract for the navigation service. `T` must implement `IViewModel`.

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

### `Navigator<T>`

Concrete implementation of `INavigator<T>`. Uses `System.Reactive` subjects internally and logs navigation transitions at `Debug` level through the injected `ILogger<Navigator<T>>`.

**Navigation actions:**

- **Push** — navigate forward while preserving history (e.g. opening a detail page).
- **Pop** — navigate back (e.g. closing a detail page).
- **Slide** — replace the current page without adding to history (e.g. switching between sibling pages in a wizard).
- **Goto** — jump to a root page and discard all history (e.g. logging out and returning to the login screen).

```csharp
// Example setup
public interface IPageViewModel : IViewModel { }

public class HomeViewModel : ViewModel, IPageViewModel { }
public class DetailViewModel : ViewModel, IPageViewModel { }

// Registration
builder.Services.AddNavigation<IPageViewModel>();
builder.Services.AddTransient<HomeViewModel>();
builder.Services.AddTransient<DetailViewModel>();

// Usage in a shell view model
public class ShellViewModel : ViewModel
{
    private readonly INavigator<IPageViewModel> _navigator;

    public INavigator<IPageViewModel> Navigator => _navigator;

    public ShellViewModel(INavigator<IPageViewModel> navigator, HomeViewModel home)
    {
        _navigator = navigator;
        _navigator.Push(home);
    }
}
```

Bind `Navigator.CurrentViewModel` in XAML with a `DataTemplate` selector or a view locator to display the correct view automatically:

```xml
<ContentControl Content="{Binding Navigator.CurrentViewModel}" />
```

### `NavigationChangedInfo<T>`

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

