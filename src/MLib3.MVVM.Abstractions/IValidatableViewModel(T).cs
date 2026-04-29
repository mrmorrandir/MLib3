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
/// classes provide the <see cref="IValidatableViewModel.SetErrors(string, string[])"/>,
/// <see cref="IValidatableViewModel.ClearErrors(string)"/> and <see cref="IValidatableViewModel.ClearErrors()"/> methods to
/// allow callers to modify the reported error state.</para>
///
/// <para>If the view model manages its own validation internally (for example, when
/// inheriting from <c>ObservableValidator</c>), prefer <see cref="IViewModelValidator"/>
/// which is tailored for internal validation logic and does not expose public
/// mutation methods for errors.</para>
///
/// <para>Implementations should raise <see cref="IValidatableViewModel.ErrorsChanged"/> and update
/// <see cref="IValidatableViewModel.HasErrors"/> appropriately when errors are set or cleared.</para>
/// </remarks>
public interface IValidatableViewModel<TModel> : IValidatableViewModel, IViewModel<TModel> where TModel : class
{
    
}