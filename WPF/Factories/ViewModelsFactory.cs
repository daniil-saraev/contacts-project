using Desktop.Stores;
using Desktop.ViewModels;
using Desktop.ViewModels.Account;
using Desktop.ViewModels.Contacts;

namespace Desktop.Factories
{
    public class ViewModelsFactory
    {
        private readonly IContactsStore _contactsStore;
        private readonly SelectedContact _selectedContact;

        public ViewModelsFactory(IContactsStore contactsStore, SelectedContact selectedContact)
        {
            _contactsStore = contactsStore;
            _selectedContact = selectedContact;
        }

        public BaseViewModel GetNewViewModel(BaseViewModel initialViewModel)
        {
            if (initialViewModel is ContactInfoViewModel)
            {
                return new ContactInfoViewModel(_selectedContact);
            }
            if (initialViewModel is LoginViewModel)
            {
                return initialViewModel;
            }

            return new HomeViewModel(_contactsStore, _selectedContact);
        }
    }
}
