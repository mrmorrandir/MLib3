using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MLib3.MVVM;

/// <summary>
/// Represents a view model class that tracks validation errors at the property level.
/// </summary>
/// <typeparam name="TModel">The type of the associated data model.</typeparam>
/// <remarks>
/// This class extends <see cref="ViewModel{TModel}"/> and implements <see cref="INotifyDataErrorInfo"/>
/// to provide validation error management functionality. It maintains internal error state for properties
/// and raises notifications when errors are updated, supporting data validation in MVVM (Model-View-ViewModel) applications.
/// </remarks>
public partial class ViewModelTracked<TModel> : ValidatableViewModel<TModel>, IViewModelTracked<TModel> where TModel : class
{
    /// <summary>
    /// Indicates whether there are unsaved changes in the view model.
    /// </summary>
    /// <remarks>
    /// This field is used to track modifications made to the associated model or data.
    /// When set to <c>true</c>, it implies that changes have been made that need to
    /// be saved or otherwise handled. Its value can be updated internally when properties
    /// of the view model change.
    /// </remarks>
    [ObservableProperty]
    private bool _hasChanges;

    /// <summary>
    /// Determines whether the associated model instance has been marked as deleted.
    /// </summary>
    /// <remarks>
    /// This field tracks the logical deletion state of the view model's associated data.
    /// A value of <c>true</c> indicates that the model has been flagged as deleted,
    /// which can be used to conditionally exclude it from operations or display.
    /// The property is managed internally and reflects the current state of the model.
    /// </remarks>
    [ObservableProperty]
    private bool _isDeleted;

    /// <summary>
    /// Determines whether the current view model is in an editable state.
    /// </summary>
    /// <remarks>
    /// This field is used to track whether the associated view model allows modifications
    /// to its data. When set to <c>true</c>, it indicates that the view model supports
    /// editing. The value of this field can be changed based on the application's logic
    /// or user interaction.
    /// </remarks>
    [ObservableProperty]
    private bool _isEditable;

    /// <summary>
    /// Represents whether the associated model is newly created and yet to be saved or processed.
    /// </summary>
    /// <remarks>
    /// This field is used to track the creation state of a model within the view model.
    /// When set to <c>true</c>, it signifies that the model is new and may not yet exist
    /// in persistent storage or the intended final state. Its value can be updated internally
    /// based on the model's lifecycle or external operations.
    /// </remarks>
    [ObservableProperty]
    private bool _isNew;

    /// <summary>
    /// Represents a value indicating whether the current item is selected.
    /// </summary>
    /// <remarks>
    /// This field is typically used to track selection state in a UI context.
    /// Setting this property can trigger UI updates or downstream logic
    /// that depends on the selection state of the associated view model.
    /// </remarks>
    [ObservableProperty]
    private bool _isSelected;

    public ViewModelTracked(TModel model) : base(model)
    {
        
    }

    /// <summary>
    /// Occurs when the validation errors for a property or the entire object change.
    /// </summary>
    /// <remarks>
    /// This event is raised to notify subscribers that the errors associated with a property
    /// (or all properties) have been updated. This is commonly used in the implementation of
    /// the <see cref="INotifyDataErrorInfo"/> interface for data validation scenarios.
    /// </remarks>
    public override event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>
    /// Updates the value of a tracked property, notifies listeners of the change,
    /// and sets a flag indicating that the state has been modified.
    /// </summary>
    /// <typeparam name="TProp">The type of the property being updated.</typeparam>
    /// <param name="oldValue">The current value of the property.</param>
    /// <param name="newValue">The new value for the property.</param>
    /// <param name="setter">An action that assigns the new value to the property.</param>
    /// <param name="propertyName">The name of the property being updated. Automatically provided by the caller if not specified.</param>
    /// <returns>Returns <c>true</c> if the property was successfully updated; otherwise, <c>false</c>.</returns>
    protected bool SetTrackedProperty<TProp>(
        TProp oldValue,
        TProp newValue,
        Action<TProp> setter,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<TProp>.Default.Equals(oldValue, newValue))
                 return false;
     
        OnPropertyChanging(propertyName);

        setter(newValue);

        OnPropertyChanged(propertyName); 
        HasChanges = true;
             
        return true;
    }
}