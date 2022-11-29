using Desktop.Common.ViewModels;

namespace Desktop.Common.Services
{
    /// <summary>
    /// Encapsulates viewmodels' creation and eliminates the need to pass a lot of dependencies.
    /// </summary>
    public interface IViewModelsFactory
    {
        public BaseViewModel GetViewModel<T>() where T : BaseViewModel;
    }
}
