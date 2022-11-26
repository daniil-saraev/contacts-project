using Desktop.Common.ViewModels;

namespace Desktop.Common.Services
{
    public interface IViewModelsFactory
    {
        public BaseViewModel GetViewModel<T>() where T : BaseViewModel;
    }
}
