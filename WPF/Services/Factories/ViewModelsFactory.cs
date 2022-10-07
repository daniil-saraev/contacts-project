using Desktop.Containers;
using Desktop.ViewModels;
using Desktop.ViewModels.Account;
using Desktop.ViewModels.Contacts;
using System.Runtime.CompilerServices;

namespace Desktop.Services.Factories
{
    public class ViewModelsFactory
    {
        private readonly IContactsStore _contactsStore;
        private readonly SelectedContact _selectedContact;
        private readonly ContactCommandsFactory _commandsFactory;

        public ViewModelsFactory(IContactsStore contactsStore, SelectedContact selectedContact, ContactCommandsFactory commandsFactory)
        {
            _contactsStore = contactsStore;
            _selectedContact = selectedContact;
            _commandsFactory = commandsFactory;
        }

        public BaseViewModel GetNewViewModel(BaseViewModel initialViewModel)
        {
            if (initialViewModel is ContactInfoViewModel)
            {
                return new ContactInfoViewModel(_selectedContact, _commandsFactory);
            }
            if (initialViewModel is LoginViewModel)
            {
                return initialViewModel;
            }

            return new HomeViewModel(_contactsStore, _selectedContact, _commandsFactory);
        }
    }
}
