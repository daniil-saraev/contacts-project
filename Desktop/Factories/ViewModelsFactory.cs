using Desktop.ViewModels;
using Desktop.ViewModels.Contacts;

namespace Desktop.Factories
{
    public static class ViewModelsFactory
    {
        public static BaseViewModel GetNewViewModel(BaseViewModel initialViewModel)
        {
            if(initialViewModel is HomeViewModel)
            {
                return new HomeViewModel();
            }
            if(initialViewModel is ContactInfoViewModel)
            {
                return new ContactInfoViewModel(((ContactInfoViewModel)initialViewModel).Contact);
            }
            if (initialViewModel is ContactEditViewModel)
            {
                return new ContactInfoViewModel(((ContactEditViewModel)initialViewModel).Contact);
            }
            if (initialViewModel is ContactAddViewModel)
            {
                return new ContactAddViewModel();
            }

            return new HomeViewModel();
        }

    }
}
