namespace MLib3.MVVM;

/// <summary>
/// Represents a generic interface for validating a strongly-typed view model.
/// Combines functionality from <see cref="IViewModel{TModel}"/> and <see cref="IViewModelValidator"/>
/// to provide property change notifications, validation error tracking, and access to the underlying model.
/// </summary>
/// <typeparam name="TModel">
/// The type of the model associated with the view model. Must be a reference type.
/// </typeparam>
/// <remarks>
/// <para>
/// This interface is intended only for view model implementations that perform validation
/// internally (that is, the view model itself is responsible for validating its model and
/// managing reported errors).
/// </para>
/// <para>
/// It is not suitable for scenarios where validation is performed externally because
/// <c>INotifyDataErrorInfo</c> does not expose public methods such as <c>SetErrors</c> or
/// <c>ClearErrors</c> that an external component could call to update the view model's
/// error state. If external validation is required (for example, when a validator
/// component needs to supply or clear errors on a view model), use <see cref="IValidatableViewModel"/> instead.
/// </para>
/// </remarks>
public interface IViewModelValidator<TModel> : IViewModelValidator, IViewModel<TModel> where TModel : class
{
}