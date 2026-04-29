using System.Collections;
using System.ComponentModel;

namespace MLib3.MVVM;

/// <summary>
/// <para>Defines a contract for a view model that supports validation and error tracking
/// when validation is performed or controlled by an external component.</para>
/// <para>Extends <see cref="IViewModel"/> and provides explicit methods to set and
/// clear validation errors from outside the view model implementation.</para>
/// </summary>
/// <remarks>
/// <para>Use this interface when an external validator or coordinator needs to supply
/// or clear validation errors on a view model (for example, in cross-field or
/// complex validation scenarios executed outside the view model). Implementing
/// classes provide the <see cref="SetErrors(string, string[])"/>,
/// <see cref="ClearErrors(string)"/> and <see cref="ClearErrors()"/> methods to
/// allow callers to modify the reported error state.</para>
///
/// <para>If the view model manages its own validation internally (for example, when
/// inheriting from <c>ObservableValidator</c>), prefer <see cref="IViewModelValidator"/>
/// which is tailored for internal validation logic and does not expose public
/// mutation methods for errors.</para>
///
/// <para>Implementations should raise <see cref="ErrorsChanged"/> and update
/// <see cref="HasErrors"/> appropriately when errors are set or cleared.</para>
/// </remarks>
public interface IValidatableViewModel : IViewModel, INotifyDataErrorInfo
{
    /// <summary>
    /// Sets or updates the validation errors associated with a specific property.
    /// Notifies listeners of changes to the errors and modifies the overall error state.
    /// </summary>
    /// <param name="propertyName">The name of the property for which errors are being set.</param>
    /// <param name="errors">A collection of error messages to associate with the property.
    /// Pass an empty array to clear errors for the property.</param>
    void SetErrors(string propertyName, params string[] errors);

    /// <summary>
    /// Clears all validation errors associated with a specific property.
    /// Notifies listeners of changes to the errors and updates the overall error state.
    /// </summary>
    /// <param name="propertyName">The name of the property for which errors should be cleared.</param>
    void ClearErrors(string propertyName);

    /// <summary>
    /// Clears all validation errors across all properties.
    /// Notifies listeners of changes to the errors and updates the overall error state.
    /// </summary>
    void ClearErrors();

    /// <summary>
    /// Retrieves the validation errors associated with the specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property for which validation errors are requested.
    /// If null or empty, returns all validation errors for the object.</param>
    /// <returns>A collection of validation errors for the specified property,
    /// or all errors if no property name is provided.</returns>
    new IEnumerable GetErrors(string? propertyName);

    /// Indicates whether the view model currently has any validation errors.
    /// This property returns true if there are errors present, otherwise false.
    /// It is often used in conjunction with data validation mechanisms to determine
    /// the validity state of the view model and its underlying data.
    new bool HasErrors { get; }

    /// Occurs when the validation errors associated with a property have changed.
    /// This event is triggered to notify listeners about modifications in the validation
    /// state of the view model, enabling the UI or other components to react accordingly.
    new event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}