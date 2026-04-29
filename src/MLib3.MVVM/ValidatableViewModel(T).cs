using System.Collections;
using System.ComponentModel;

namespace MLib3.MVVM;

/// <summary>
/// <para>Provides a concrete view model implementation that supports external
/// validation for a model of type <typeparamref name="TModel"/>.</para>
/// <para>This class implements <see cref="IValidatableViewModel{TModel}"/>,
/// exposes methods to set and clear validation errors from outside the view
/// model (<see cref="SetErrors(string, string[])"/>,
/// <see cref="ClearErrors(string)"/>, <see cref="ClearErrors()"/>) and
/// implements the <see cref="INotifyDataErrorInfo"/> pattern for data binding.
/// </para>
/// </summary>
/// <typeparam name="TModel">The type of the model associated with the view model. Must be a reference type.</typeparam>
/// <remarks>
/// <para>The class stores validation errors in an internal dictionary keyed by
/// property name and raises <see cref="ErrorsChanged"/> when errors change.
/// Consumers (for example, external validator components) can call
/// <see cref="SetErrors(string, string[])"/>, <see cref="ClearErrors(string)"/>
/// or <see cref="ClearErrors()"/> to report and remove validation errors.
/// </para>
///
/// <para>If the view model itself manages validation logic internally (for
/// example by inheriting from a validation-capable base such as
/// <c>ObservableValidator</c>), prefer using an implementation of
/// <see cref="IViewModelValidator{TModel}"/> which is intended for internal
/// validation scenarios and typically does not expose public mutation methods
/// for errors.</para>
///
/// <para>Note: the class raises <see cref="ErrorsChanged"/> and updates the
/// <see cref="HasErrors"/> state; callers should ensure any cross-thread
/// notifications are marshalled to the UI thread if necessary. Also consider
/// unsubscribing from events when view models have non-trivial lifetimes to
/// avoid memory leaks.</para>
/// </remarks>
public class ValidatableViewModel<TModel> : ValidatableViewModel, IViewModel<TModel>, IValidatableViewModel<TModel> where TModel : class
{
    /// <summary>
    /// Represents the underlying model associated with the view model.
    /// </summary>
    /// <remarks>
    /// This property provides access to the core data or business object that the view model encapsulates.
    /// It acts as a bridge between the model and the view, allowing for MVVM-based data binding and interaction.
    /// Changes to this property may trigger updates in dependent properties or validation logic in the view model.
    /// </remarks>
    public TModel Model { get; set; }

    protected ValidatableViewModel(TModel model) 
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }
}