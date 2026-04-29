using System.ComponentModel;

namespace MLib3.MVVM;

/// <summary>
/// Represents a contract for a view model that tracks validation errors,
/// state changes, and provides additional information regarding its editability,
/// selection, and lifecycle state.
/// </summary>
/// <remarks>
/// This interface extends the INotifyDataErrorInfo interface to include methods for managing validation errors,
/// as well as properties for tracking changes, deletion state, editability, newness, and selection state of the view model.
/// Implementing this interface allows for consistent handling of validation and state management across view models in an MVVM architecture.
/// </remarks>
public interface IViewModelTracked : INotifyDataErrorInfo
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
    /// Gets or sets a value indicating whether the object's state has been modified
    /// since it was last loaded or saved. This property can be used to track changes
    /// for implementation of features such as change detection, save prompts, or undo/redo operations.
    /// </summary>
    bool HasChanges { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance is marked as deleted.
    /// This property can be used to track and manage the deletion state of the object,
    /// enabling proper handling within the application logic.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance is in an editable state.
    /// This property determines if the object's properties can be modified,
    /// often used to control UI or validation logic based on the editability of the instance.
    /// </summary>
    bool IsEditable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance represents a newly created object
    /// that has not yet been persisted or fully initialized within the application.
    /// This property can be useful for determining and handling objects that are in their initial state.
    /// </summary>
    bool IsNew { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance is selected.
    /// This property can be used to track the selection state of the object,
    /// supporting logic for user interactions or data processing requiring selection awareness.
    /// </summary>
    bool IsSelected { get; set; }
}