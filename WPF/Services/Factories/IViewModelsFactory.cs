using Desktop.ViewModels;

namespace Desktop.Services.Factories
{
    public interface IViewModelsFactory
    {
        public BaseViewModel GetViewModel<T>() where T : BaseViewModel;
    }
}
