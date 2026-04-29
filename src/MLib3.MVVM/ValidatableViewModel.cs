using System.Collections;
using System.ComponentModel;

namespace MLib3.MVVM;

/// <summary>
/// <para>Provides a concrete (abstract) view model base that supports external
/// validation and error tracking.</para>
/// <para>This class implements <see cref="IValidatableViewModel"/>, exposes
/// methods to set and clear validation errors from outside the view model
/// implementation (<see cref="SetErrors(string, string[])"/>,
/// <see cref="ClearErrors(string)"/>, <see cref="ClearErrors()"/>) and
/// participates in the <see cref="INotifyDataErrorInfo"/> pattern for data
/// binding.</para>
/// </summary>
/// <remarks>
/// <para>The class stores validation errors in an internal dictionary keyed by
/// property name and raises <see cref="ErrorsChanged"/> when errors change.
/// External components (for example, validator services) can call
/// <see cref="SetErrors(string, string[])"/>, <see cref="ClearErrors(string)"/>
/// or <see cref="ClearErrors()"/> to report and remove validation errors.</para>
///
/// <para>If the view model manages its own validation internally (for example by
/// inheriting from a validation-capable base such as <c>ObservableValidator</c>),
/// prefer using an implementation of <see cref="IViewModelValidator"/>
/// which is intended for internal validation scenarios and typically does not
/// expose public mutation methods for errors.</para>
///
/// <para>Note: this class raises <see cref="ErrorsChanged"/> and updates the
/// <see cref="HasErrors"/> state; callers should ensure any cross-thread
/// notifications are marshalled to the UI thread if necessary. Also consider
/// unsubscribing from events when view models have non-trivial lifetimes to
/// avoid memory leaks.</para>
/// </remarks>
public abstract class ValidatableViewModel : ViewModel, IValidatableViewModel
{
    /// <summary>
    ///     Holds validation error messages for properties in the view model.
    /// </summary>
    /// <remarks>
    ///     This dictionary is keyed by property names, with each key mapping to a list
    ///     of corresponding error messages. It is used internally to manage and track
    ///     data validation errors. Changes to this field trigger error notifications
    ///     and updates to the <c>HasErrors</c> property.
    /// </remarks>
    protected readonly Dictionary<string, List<string>> _errors = new();

    /// <summary>
    ///     Gets a value indicating whether the object has any validation errors.
    /// </summary>
    /// <remarks>
    ///     This property returns <c>true</c> if there are validation errors for any properties
    ///     in the object; otherwise, it returns <c>false</c>. It is commonly used to determine
    ///     the overall validation state of the object when implementing
    ///     the <see cref="INotifyDataErrorInfo" /> interface.
    /// </remarks>
    public bool HasErrors => _errors.Count > 0;

    /// <summary>
    ///     Occurs when the validation errors for a property or the entire object change.
    /// </summary>
    /// <remarks>
    ///     This event is raised to notify subscribers that the errors associated with a property
    ///     (or all properties) have been updated. This is commonly used in the implementation of
    ///     the <see cref="INotifyDataErrorInfo" /> interface for data validation scenarios.
    /// </remarks>
    public virtual event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>
    ///     Retrieves the validation errors associated with a specific property or all properties.
    /// </summary>
    /// <param name="propertyName">
    ///     The name of the property for which errors are being retrieved.
    ///     If null or empty, errors for all properties are returned.
    /// </param>
    /// <returns>
    ///     An enumerable collection of validation error messages.
    ///     If no errors exist, an empty collection is returned.
    /// </returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return _errors.SelectMany(kv => kv.Value);

        return _errors.TryGetValue(propertyName, out var list) ? list : Enumerable.Empty<string>();
    }

    /// <summary>
    ///     Sets or updates the validation errors associated with a specific property.
    ///     Notifies listeners of changes to the errors and modifies the overall error state.
    /// </summary>
    /// <param name="propertyName">The name of the property for which errors are being set.</param>
    /// <param name="errors">
    ///     A collection of error messages to associate with the property.
    ///     Pass an empty array to clear errors for the property.
    /// </param>
    public void SetErrors(string propertyName, params string[] errors)
    {
        var newList = errors?.Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().ToList() ?? new List<string>();

        // Nur ändern, wenn es echte Änderungen gibt (reduziert Flickern/Events)
        if (_errors.TryGetValue(propertyName, out var existing))
        {
            if (existing.SequenceEqual(newList))
                return;

            if (newList.Count == 0)
                _errors.Remove(propertyName);
            else
                _errors[propertyName] = newList;
        }
        else
        {
            if (newList.Count == 0)
                return;

            _errors[propertyName] = newList;
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
    }

    /// <summary>
    ///     Clears all validation errors associated with a specific property.
    ///     Notifies listeners of changes to the errors and updates the overall error state.
    /// </summary>
    /// <param name="propertyName">The name of the property for which errors should be cleared.</param>
    public void ClearErrors(string propertyName) => SetErrors(propertyName);

    /// <summary>
    ///     Clears all validation errors across all properties.
    ///     Notifies listeners of changes to the errors and updates the overall error state.
    /// </summary>
    public void ClearErrors()
    {
        _errors.Clear();
        OnPropertyChanged(nameof(HasErrors));
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(null));
    }
}