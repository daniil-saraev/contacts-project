namespace Desktop.App.Services.Factories
{
    public interface IViewModelsFactory
    {
        public BaseViewModel GetViewModel<T>() where T : BaseViewModel;
    }
}
