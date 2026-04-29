using System.ComponentModel;

namespace MLib3.MVVM;

/// <summary>
/// Represents an interface for validating a view model.
/// Combines the capabilities of the <see cref="IViewModel"/> interface and
/// <see cref="INotifyDataErrorInfo"/> to provide support for property change
/// notifications and validation error tracking.
/// </summary>
/// <remarks>
/// <para>
/// This interface is intended for view model implementations that perform
/// validation internally (for example, classes that inherit from
/// <c>ObservableValidator</c>). It assumes the view model itself manages the
/// validation lifecycle and the reporting of errors via
/// <see cref="INotifyDataErrorInfo"/>.
/// </para>
/// <para>
/// It is not suitable for scenarios where validation is performed by an
/// external component, because <c>INotifyDataErrorInfo</c> does not expose
/// public methods such as <c>SetErrors</c> or <c>ClearErrors</c> that an
/// external validator could call to update the view model's error state.
/// For external validation scenarios use <see cref="IValidatableViewModel"/>,
/// which provides explicit methods to set and clear errors from outside the
/// view model implementation.
/// </para>
/// </remarks>
public interface IViewModelValidator: IViewModel, INotifyDataErrorInfo
{
    
}